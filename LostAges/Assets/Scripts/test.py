# daily_briefing_local.py

import datetime
import requests
from fpdf import FPDF
import os
import random

# ==== Einstellungen ====
GOOGLE_API_KEY = 'DEIN_GOOGLE_API_KEY'
MICROSOFT_ACCESS_TOKEN = 'DEIN_MICROSOFT_ACCESS_TOKEN'
OPENWEATHER_API_KEY = 'DEIN_OPENWEATHER_API_KEY'
CITY = 'Nürnberg'  # Dein Standort

# ==== 1. Google Kalender: Termine holen ====
def get_google_calendar_events():
    heute = datetime.datetime.utcnow().isoformat() + 'Z'
    morgen = (datetime.datetime.utcnow() + datetime.timedelta(days=2)).isoformat() + 'Z'
    url = f'https://www.googleapis.com/calendar/v3/calendars/primary/events?timeMin={heute}&timeMax={morgen}&singleEvents=true&orderBy=startTime&key={GOOGLE_API_KEY}'
    response = requests.get(url)
    events = response.json().get('items', [])
    termine = []
    for event in events:
        start = event['start'].get('dateTime', event['start'].get('date'))
        summary = event.get('summary', 'Kein Titel')
        termine.append(f"{start}: {summary}")
    return termine

# ==== 2. Microsoft To Do: Aufgaben holen ====
def get_microsoft_todo_tasks():
    headers = {'Authorization': f'Bearer {MICROSOFT_ACCESS_TOKEN}'}
    url = 'https://graph.microsoft.com/v1.0/me/todo/lists'
    lists = requests.get(url, headers=headers).json()['value']
    aufgaben = []
    for l in lists:
        list_id = l['id']
        tasks_url = f'https://graph.microsoft.com/v1.0/me/todo/lists/{list_id}/tasks'
        tasks = requests.get(tasks_url, headers=headers).json().get('value', [])
        for task in tasks:
            if not task.get('completedDateTime'):
                due = task.get('dueDateTime', {}).get('dateTime', None)
                if due:
                    aufgaben.append(f"{due}: {task['title']}")
    return aufgaben

# ==== 3. Wetter abrufen ====
def get_weather():
    url = f"https://api.openweathermap.org/data/2.5/weather?q={CITY}&appid={OPENWEATHER_API_KEY}&units=metric&lang=de"
    response = requests.get(url).json()
    beschreibung = response['weather'][0]['description']
    temp = response['main']['temp']
    return f"{CITY}: {beschreibung}, {temp}°C"

# ==== 4. Motivationszitat ====
def get_motivation_quote():
    quotes = [
        "Gib niemals auf!",
        "Heute ist ein guter Tag, um großartig zu sein!",
        "Dein Erfolg beginnt mit deinem Mut.",
        "Mach jeden Tag zu einem Meisterwerk."
    ]
    return random.choice(quotes)

# ==== 5. PDF erstellen ====
def create_pdf(termine, aufgaben, wetter, zitat, filename='briefing.pdf'):
    pdf = FPDF()
    pdf.add_page()
    pdf.set_font("Arial", size=12)

    pdf.cell(200, 10, txt="Tägliches Briefing", ln=True, align='C')
    pdf.ln(10)

    pdf.cell(200, 10, txt="Termine:", ln=True)
    pdf.ln(5)
    for t in termine:
        pdf.multi_cell(0, 10, t)

    pdf.ln(10)
    pdf.cell(200, 10, txt="Aufgaben:", ln=True)
    pdf.ln(5)
    for a in aufgaben:
        pdf.multi_cell(0, 10, a)

    pdf.ln(10)
    pdf.cell(200, 10, txt="Wetter:", ln=True)
    pdf.ln(5)
    pdf.multi_cell(0, 10, wetter)

    pdf.ln(10)
    pdf.cell(200, 10, txt="Motivationszitat:", ln=True)
    pdf.ln(5)
    pdf.multi_cell(0, 10, zitat)

    pdf.output(filename)
    print(f"PDF erfolgreich gespeichert unter: {os.path.abspath(filename)}")

# ==== Hauptfunktion ====
def main():
    termine = get_google_calendar_events()
    aufgaben = get_microsoft_todo_tasks()
    wetter = get_weather()
    zitat = get_motivation_quote()

    create_pdf(termine, aufgaben, wetter, zitat)

if __name__ == "__main__":
    main()
