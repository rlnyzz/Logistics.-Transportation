import pandas as pd
from sklearn.model_selection import train_test_split, cross_val_score
from sklearn.pipeline import Pipeline
from sklearn.feature_extraction.text import TfidfVectorizer
from sklearn.svm import LinearSVC
from sklearn.calibration import CalibratedClassifierCV
from sklearn.utils.class_weight import compute_class_weight
from collections import Counter
import joblib
import numpy as np

print("Loading dataset...")

# Нормализация — убираем только мусорные слова, НЕ трогаем смысловые
STOP_WORDS = {"срочно", "быстрая", "логистика", "транспортировка", "поставка", "оптом", "груз"}

def normalize(text):
    text = str(text).lower().strip()
    words = text.split()
    # Фильтруем только явный мусор
    words = [w for w in words if w not in STOP_WORDS]
    return " ".join(words)

df = pd.read_csv("dataset_rus_description_result.csv")
df["description"] = df["description"].apply(normalize)

# Посмотрим на баланс классов
print("\nРаспределение классов:")
print(Counter(df["result"]))

X = df["description"]
y = df["result"]

X_train, X_test, y_train, y_test = train_test_split(
    X, y, test_size=0.2, random_state=42, stratify=y
)

print(f"\nТренировочных примеров: {len(X_train)}")
print(f"Тестовых примеров: {len(X_test)}")

# Веса классов для баланса
classes = np.unique(y_train)
weights = compute_class_weight("balanced", classes=classes, y=y_train)
class_weight = dict(zip(classes, weights))
print(f"\nВеса классов: {class_weight}")

print("\nTraining model...")

model = Pipeline([
    ("tfidf", TfidfVectorizer(
        ngram_range=(1, 3),
        min_df=1,              # min_df=1 чтобы не терять редкие слова
        max_features=50000,
        sublinear_tf=True,     # логарифмическое TF — улучшает качество
        analyzer="word",
    )),
    ("clf", CalibratedClassifierCV(
        LinearSVC(
            class_weight="balanced",  # автоматический баланс классов
            C=1.0,
            max_iter=2000
        ),
        cv=5
    ))
])

model.fit(X_train, y_train)

accuracy = model.score(X_test, y_test)
print(f"\nACCURACY: {accuracy:.4f}")

# Кросс-валидация для честной оценки
cv_scores = cross_val_score(model, X, y, cv=5, scoring="accuracy")
print(f"Cross-val accuracy: {cv_scores.mean():.4f} ± {cv_scores.std():.4f}")

# Проверим проблемные примеры
print("\n--- Тест проблемных запросов ---")
test_cases = [
    "доставка дивана",
    "напитки",
    "диван",
    "вода",
    "соки",
    "мясо",
    "телефон",
    "песок",
    "цемент",
    "кровать",
    "йогурт",
    "кирпич",
    "ноутбук",
    "мороженое",
    "щебень",
]

for case in test_cases:
    normalized = normalize(case)
    pred = model.predict([normalized])[0]
    probs = model.predict_proba([normalized])[0]
    confidence = float(max(probs))
    print(f"'{case}' → {pred} ({confidence:.2f})")

joblib.dump(model, "model.pkl")
print("\nModel saved!")