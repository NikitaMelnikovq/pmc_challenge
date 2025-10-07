import "./CheckoutPageMain.css";
import { useRef, useMemo } from "react";
import { useCartStore } from "../../store/useCartStore";

const CheckoutPageMain = () => {
    const formRef = useRef<HTMLFormElement | null>(null);

    const items = useCartStore((s) => s.items);
    const clear = useCartStore((s) => s.clear);

    // TODO: цены — когда появится backend, сюда придёт реальный расчёт
    const totals = useMemo(() => {
        const positions = items.length;

        // пример заготовки под будущие суммы:
        // const sum = items.reduce((acc, it) => {
        //   const unitPrice = getPriceForUnit(it); // по правилам ТЗ (динамические пороги)
        //   return acc + unitPrice * it.qty;
        // }, 0);

        return {
            positions,
            sumLabel: "—",     // заменить на formatCurrency(sum)
            discountLabel: "—",// если будет скидка
            totalLabel: "—",   // заменить на formatCurrency(sum - discount)
        };
    }, [items]);

    const onSubmit = (e: React.FormEvent) => {
        e.preventDefault();

        if (!formRef.current) return;
        // HTML5-валидация
        const isValid = formRef.current.reportValidity();
        if (!isValid) return;

        if (items.length === 0) {
            alert("Корзина пуста — добавьте товары перед оформлением.");
            return;
        }

        // Здесь отправка на backend:
        // await api.post('/orders', { customer: formData, items })

        alert("✅ Заказ готов");
        clear();
        // По желанию: редирект в каталог
        // window.history.pushState({}, "", "/catalog");
        // window.dispatchEvent(new PopStateEvent("popstate"));
    };

    return (
        <div className="catalog-container">
            <main className="checkout">
                <h2 className="checkout__title">Оформление заказа</h2>

                <section className="checkout__grid">
                    <form className="form" ref={formRef} onSubmit={onSubmit}>
                        <div className="form__row">
                            <label className="form__label" htmlFor="firstName">Имя</label>
                            <input id="firstName" className="form__input" placeholder="Иван" required />
                        </div>

                        <div className="form__row">
                            <label className="form__label" htmlFor="lastName">Фамилия</label>
                            <input id="lastName" className="form__input" placeholder="Иванов" required />
                        </div>

                        <div className="form__row">
                            <label className="form__label" htmlFor="inn">ИНН</label>
                            <input
                                id="inn"
                                className="form__input"
                                placeholder="XXXXXXXXXX"
                                inputMode="numeric"
                                pattern="[0-9]{10,12}"
                                title="Введите 10 или 12 цифр"
                                required
                            />
                        </div>

                        <div className="form__row">
                            <label className="form__label" htmlFor="phone">Телефон</label>
                            <input
                                id="phone"
                                className="form__input"
                                placeholder="+7 XXX XXX-XX-XX"
                                inputMode="tel"
                                pattern="[\+\d][\d\ \-\(\)]{9,}"
                                title="Введите корректный номер телефона"
                                required
                            />
                        </div>

                        <div className="form__row">
                            <label className="form__label" htmlFor="email">Email</label>
                            <input
                                id="email"
                                className="form__input"
                                placeholder="name@mail.ru"
                                type="email"
                                required
                            />
                        </div>

                        <div className="form__row">
                            <label className="form__label" htmlFor="comment">Комментарий (опционально)</label>
                            <textarea id="comment" className="form__input form__textarea" placeholder="Пожелания к заказу" />
                        </div>

                        <button className="btn btn--primary btn--full" type="submit">
                            Подтвердить заказ
                        </button>
                    </form>

                    <aside className="summary">
                        <div className="summary__title">Итоги</div>

                        {/* список позиций из корзины */}
                        <div className="summary__items">
                            {items.length === 0 ? (
                                <div className="summary__empty">Корзина пуста</div>
                            ) : (
                                items.map((i) => (
                                    <div className="summary__item" key={i.id}>
                                        <div className="summary__itemName">{i.product.Name}</div>
                                        <div className="summary__itemRow">
                                            <span>Кол-во:</span>
                                            <span>{i.qty} {i.unit === "m" ? "м" : "т"}</span>
                                        </div>
                                        <div className="summary__itemRow">
                                            <span>Цена:</span>
                                            <span>—</span> {/* TODO: сюда подставим рассчитанную цену за выбранную единицу */}
                                        </div>
                                    </div>
                                ))
                            )}
                        </div>

                        {/* сводка */}
                        <div className="summary__line">
                            <span>Позиции</span><span>{totals.positions}</span>
                        </div>
                        <div className="summary__line">
                            <span>Сумма</span><span>{totals.sumLabel}</span>
                        </div>
                        <div className="summary__line">
                            <span>Скидка</span><span>{totals.discountLabel}</span>
                        </div>
                        <div className="summary__line summary__line--total">
                            <span>К оплате</span><span>{totals.totalLabel}</span>
                        </div>
                    </aside>
                </section>
            </main>
        </div>
    );
};

export default CheckoutPageMain;
