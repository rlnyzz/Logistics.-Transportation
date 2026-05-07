import pandas as pd
import random

cars = {
    "Small van":      {"base": 300,  "km": 18},
    "Medium van":     {"base": 500,  "km": 25},
    "Furniture van":  {"base": 700,  "km": 30},
    "Isothermal van": {"base": 900,  "km": 35},
    "Refrigerator":   {"base": 1200, "km": 45},
    "Tented truck":   {"base": 1500, "km": 55},
    "Dump truck":     {"base": 1800, "km": 60},
}

rows = []

for _ in range(20000):

    car = random.choice(list(cars.keys()))

    distance = random.randint(1, 200)
    duration = int(distance * random.uniform(1.2, 2.2))

    weight = random.randint(10, 10000)
    volume = random.randint(1, 60)

    base = cars[car]["base"]
    km_price = cars[car]["km"]

    # ===== цена =====
    price = base
    price += distance * km_price
    price += weight * 0.5
    price += volume * 30
    price += duration * 2

    # шум рынка
    price *= random.uniform(0.9, 1.2)

    # округление (как ты просил — без длинных чисел)
    price = round(price)

    rows.append([
        distance,
        duration,
        weight,
        volume,
        car,
        price
    ])

df = pd.DataFrame(rows, columns=[
    "DistanceKm",
    "DurationMin",
    "Weight",
    "Volume",
    "CarType",
    "PriceRub"
])

df.to_csv("dataset_price_clean.csv", index=False)

print("DONE:", len(df))
print(df.head())