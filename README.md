# 🏗️ PMC Challenge — Telegram Mini App для ТМК

> MVP-решение для автоматизации заказов трубной продукции  
> в рамках хакатона **РадиоХак 2.0 | ИРИТ-РТФ | Трубная Металлургическая Компания**

---

## 🧑‍💻 Состав команды

1. Шаматов Рафаэль Рафитович - аналитик
2. Тришина Ульяна Андреевна - разработчик телеграм бота
3. Устинов Даниил Николаевич - фронтенд разработчик, дизайнер
4. Юшков Юрий Александрович - бэкенд разработчик
5. Мельников Никита Сергеевич - бэкенд разработчик

---

## 📘 Описание проекта

Проект представляет собой экосистему из трёх модулей:
1. 🤖 **Telegram Bot** — точка входа для пользователей, через которую Mini App открывается прямо в Telegram.  
2. 💻 **Backend (C# / ASP.NET Core)** — отвечает за хранение и обработку данных: номенклатура, склады, цены, остатки, заказы.  
3. 🖥️ **Frontend (React + TypeScript)** — Telegram Mini App, удобный интерфейс для оформления заказов.

---

## 🎯 Цель решения

Создать удобный мобильный канал взаимодействия с клиентами ТМК:
- 🔍 фильтрация продукции по множественным критериям (склад, тип, ГОСТ, диаметр и др.);
- 🧺 формирование корзины (в метрах и тоннах);
- 💸 применение динамических скидок в зависимости от объёма заказа;
- 📈 получение актуальных цен, обновляющихся несколько раз в день;
- 🧾 оформление заказа с вводом персональных данных.

---

## 🧩 Архитектура проекта

```
pmc_challenge/
├─ bot/        # Telegram Bot backend (логика общения с пользователем)
├─ backend/    # ASP.NET Core (C#) API — каталоги, цены, остатки, заказы
└─ frontend/   # React + TypeScript + Vite — Telegram Mini App
```

📡 **Связь между компонентами:**

```
FRONT (React Mini App)
        ⬆️⬇️ HTTPS
   TGbot backend (Node/Python)
        ⬆️⬇️ REST API
   C# backend (ASP.NET Core)
```

---

## 🧰 Технологический стек

| Компонент | Технологии |
|------------|-------------|
| Backend | C#, ASP.NET Core, Entity Framework |
| Bot | Python / aiogram |
| Frontend | React, TypeScript, Vite, Zustand, Axios, SCSS |
| Базы данных | PostgreSQL / MSSQL |
| Контейнеризация | Docker, Docker Compose |
| Инфраструктура | Ngrok / Nginx для HTTPS при демо |
| Контроль версий | GitHub |

---

## 🚀 Как запустить проект локально

```bash
# клонируем репозиторий
git clone https://github.com/NikitaMelnikovq/pmc_challenge.git
cd pmc_challenge

# запускаем нужный модуль
cd backend   # или cd frontend, cd bot
```

### ▶️ Frontend

```bash
cd frontend
npm install
npm run dev
```
затем открыть [http://localhost:5173](http://localhost:5173)

### ▶️ Backend

```bash
cd backend
dotnet restore
dotnet run
```

### ▶️ Telegram Bot

```bash
# Переход в папку бота
cd bot_gui

# Создание виртуального окружения
python -m venv venv

# Активация виртуального окружения
# Для Windows:
venv\Scripts\activate
# Для Linux/Mac:
source venv/bin/activate

# Установка зависимостей
pip install -r requirements.txt

# Создание папки
mkdir .env

# Настройка конфигурации c токеном и URL
echo "BOT_TOKEN=your_bot_token" > .env  # Указать ваш токен
echo "WEBAPP_URL=http://localhost:3000" >> .env # Указать ваш URL
```

---

## 🔐 Авторизация через Telegram

Все запросы с фронта сопровождаются заголовком:

```
X-Telegram-Init-Data: window.Telegram.WebApp.initData
```

Бэкэнд валидирует подпись `initData` через HMAC-SHA256 с токеном бота.  
Это гарантирует безопасность и заменяет логин/пароль.
