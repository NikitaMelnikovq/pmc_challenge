import "./CheckoutPageMain.css";

const CheckoutPageMain = () => {
    return (
        <div className="catalog-container">
            <main className="checkout">
                <h2 className="checkout__title">Оформление заказа</h2>

                <section className="checkout__grid">
                    <form className="form">
                        <div className="form__row">
                            <label className="form__label">Имя</label>
                            <input className="form__input" placeholder="Иван" />
                        </div>
                        <div className="form__row">
                            <label className="form__label">Фамилия</label>
                            <input className="form__input" placeholder="Иванов" />
                        </div>
                        <div className="form__row">
                            <label className="form__label">ИНН</label>
                            <input className="form__input" placeholder="XXXXXXXXXX" />
                        </div>
                        <div className="form__row">
                            <label className="form__label">Телефон</label>
                            <input className="form__input" placeholder="+7 XXX XXX-XX-XX" />
                        </div>
                        <div className="form__row">
                            <label className="form__label">Email</label>
                            <input className="form__input" placeholder="name@mail.ru" />
                        </div>
                        <div className="form__row">
                            <label className="form__label">Комментарий (опционально)</label>
                            <textarea className="form__input form__textarea" placeholder="Пожелания к заказу" />
                        </div>

                        <button className="btn btn--primary btn--full" type="button" disabled>
                            Подтвердить заказ
                        </button>
                    </form>

                    <aside className="summary">
                        <div className="summary__title">Итоги</div>
                        <div className="summary__line">
                            <span>Позиции</span><span>2</span>
                        </div>
                        <div className="summary__line">
                            <span>Сумма</span><span>136 000 ₽</span>
                        </div>
                        <div className="summary__line">
                            <span>Скидка</span><span>− 8 000 ₽</span>
                        </div>
                        <div className="summary__line summary__line--total">
                            <span>К оплате</span><span>128 000 ₽</span>
                        </div>
                    </aside>
                </section>
            </main>
        </div>
    );
};

export default CheckoutPageMain;
