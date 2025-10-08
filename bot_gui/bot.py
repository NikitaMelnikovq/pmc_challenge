import asyncio
import logging
import os
from aiogram import Bot, Dispatcher, types
from aiogram.filters import CommandStart
from aiogram.types import ReplyKeyboardMarkup, KeyboardButton, WebAppInfo
from dotenv import load_dotenv

load_dotenv()
BOT_TOKEN = os.getenv('BOT_TOKEN')
WEBAPP_URL = os.getenv('WEBAPP_URL')

# Инициализация бота и диспетчера
bot = Bot(token=BOT_TOKEN)
dp = Dispatcher()

# Обработчик команды /start
@dp.message(CommandStart())
async def command_start_handler(message: types.Message) -> None:
    # Создаем клавиатуру
    keyboard = ReplyKeyboardMarkup(
        keyboard=[
            [KeyboardButton(text="🚀 Открыть Mini App", web_app=WebAppInfo(url=WEBAPP_URL))]
        ],
        resize_keyboard=True,
        one_time_keyboard=True
    )

    await message.answer(
        "👋 Добро пожаловать! " \
        "Мы — ТМК, ведущий поставщик трубных решений для нефтегазовой и других отраслей. " \
        "У нас вы можете заказать полный ассортимент высококачественных труб и комплексные сервисы. \n \n" \
        "⚙ Наш бот поможет найти, выбрать и оформить заказ трубной продукции. \n \n" \
        "📌 Подробнее о нас: https://www.tmk-group.ru/ \n \n" \
        "📩 Чтобы перейти в приложение нажмите на кнопку в меню."
        ,
        reply_markup=keyboard
    )

async def main() -> None:
    logging.basicConfig(level=logging.INFO)
    await dp.start_polling(bot)

if __name__ == "__main__":
    asyncio.run(main())