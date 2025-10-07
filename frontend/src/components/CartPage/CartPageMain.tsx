import "./CartPageMain.css";
import { useCartStore } from "../../store/useCartStore";

const CartPageMain = () => {
    const items = useCartStore(s => s.items);
    const updateQty = useCartStore(s => s.updateQty);
    const removeItem = useCartStore(s => s.removeItem);
    const clear = useCartStore(s => s.clear);

    const inc = (id: string | number, qty: number, unit: "m" | "t") =>
        updateQty(id, qty + (unit === "m" ? 1 : 0.01), unit);
    const dec = (id: string | number, qty: number, unit: "m" | "t") =>
        updateQty(id, Math.max(0, qty - (unit === "m" ? 1 : 0.01)), unit);

    return (
        <div className="catalog-container">
            <main className="cart">
                <h2 className="cart__title">Корзина</h2>

                {items.length === 0 ? (
                    <div className="cart__empty">Корзина пуста</div>
                ) : (
                    <>
                        <section className="cart__list">
                            {items.map((i) => (
                                <article className="cartItem" key={i.id}>
                                    <div className="cartItem__info">
                                        <div className="cartItem__name">{i.product.Name}</div>
                                        <div className="cartItem__meta">
                                            <span>Ø {i.product.Diameter}</span>
                                            <span>стенка {i.product.PipeWallThickness}</span>
                                            <span>ГОСТ {i.product.Gost}</span>
                                            <span>Марка стали: {i.product.SteelGrade}</span>
                                        </div>

                                        <div className="cartItem__qty">
                                            <div className="qtyBox">
                                                <div className="qtyBox__label">Кол-во ({i.unit === "m" ? "м" : "т"})</div>
                                                <div className="qtyBox__controls">
                                                    <button className="qtyBox__btn" onClick={() => dec(i.id, i.qty, i.unit)}>−</button>
                                                    <input
                                                        className="qtyBox__input"
                                                        value={i.qty}
                                                        onChange={(e) => {
                                                            const v = Number(e.target.value.replace(",", "."));
                                                            if (!Number.isNaN(v)) updateQty(i.id, v, i.unit);
                                                        }}
                                                    />
                                                    <button className="qtyBox__btn" onClick={() => inc(i.id, i.qty, i.unit)}>+</button>
                                                </div>
                                                <div className="qtyBox__hint">Склад ID: {String(i.stockId)}</div>
                                            </div>
                                        </div>
                                    </div>

                                    <div className="cartItem__price">
                                        <div className="priceBox">
                                            <div className="priceBox__label">Цена (заглушка)</div>
                                            <div className="priceBox__value">—</div>
                                        </div>
                                        <button className="btn btn--ghost btn--full" onClick={() => removeItem(i.id)}>
                                            Удалить
                                        </button>
                                    </div>
                                </article>
                            ))}
                        </section>

                        <section className="cart__summary">
                            <div className="summary__row">
                                <span>Позиции</span>
                                <span className="summary__value">{items.length}</span>
                            </div>
                            <a
                                className="btn btn--primary btn--full summary__cta"
                                href="/checkout"
                                onClick={(e) => { e.preventDefault(); window.history.pushState({}, "", "/checkout"); window.dispatchEvent(new PopStateEvent("popstate")); }}
                            >
                                Перейти к оформлению
                            </a>
                            <button className="btn btn--ghost btn--full" onClick={clear} style={{ marginTop: 8 }}>
                                Очистить корзину
                            </button>
                        </section>
                    </>
                )}
            </main>
        </div>
    );
};

export default CartPageMain;
