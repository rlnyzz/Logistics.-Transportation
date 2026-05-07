from fastapi import FastAPI
import joblib

app = FastAPI()

model = joblib.load("model.pkl")

@app.get("/predict")
def predict(text: str):

    pred = model.predict([text])[0]
    probs = model.predict_proba([text])[0]
    confidence = float(max(probs))

    return {
        "vehicle": pred,
        "confidence": confidence
    }