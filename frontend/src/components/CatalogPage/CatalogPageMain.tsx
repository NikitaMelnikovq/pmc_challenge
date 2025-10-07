import "./CatalogPageMain.css";
import { useState } from "react";
import { useCartStore } from "../../store/useCartStore";
import type { Nomenclature } from "../../types/product";

const mockProducts: Nomenclature[] = [
    {
        ID: 1, IDCat: 10, IDType: 100,
        Name: "Труба стальная 57×3.5 ГОСТ 8732-78",
        Gost: "8732-78", Manufacturer: "ТМК", SteelGrade: "20",
        Diameter: 57, PipeWallThickness: 3.5, Status: true, Koef: 0.04,
    },
    {
        ID: 2, IDCat: 10, IDType: 100,
        Name: "Труба стальная 76×4 ГОСТ 8732-78",
        Gost: "8732-78", Manufacturer: "ТМК", SteelGrade: "20",
        Diameter: 76, PipeWallThickness: 4, Status: true, Koef: 0.055,
    },
    {
        ID: 3, IDCat: 10, IDType: 100,
        Name: "Труба стальная 89×4.5 ГОСТ 8732-78",
        Gost: "8732-78", Manufacturer: "ЧТПЗ", SteelGrade: "09Г2С",
        Diameter: 89, PipeWallThickness: 4.5, Status: true, Koef: 0.075,
    },
    {
        ID: 4, IDCat: 10, IDType: 100,
        Name: "Труба стальная 108×5 ГОСТ 8732-78",
        Gost: "8732-78", Manufacturer: "ВТЗ", SteelGrade: "20",
        Diameter: 108, PipeWallThickness: 5, Status: true, Koef: 0.095,
    },
    {
        ID: 5, IDCat: 10, IDType: 100,
        Name: "Труба стальная 133×6 ГОСТ 8732-78",
        Gost: "8732-78", Manufacturer: "ТМК", SteelGrade: "17Г1С",
        Diameter: 133, PipeWallThickness: 6, Status: true, Koef: 0.125,
    },
    {
        ID: 6, IDCat: 10, IDType: 100,
        Name: "Труба стальная 159×6 ГОСТ 8732-78",
        Gost: "8732-78", Manufacturer: "ЧТПЗ", SteelGrade: "20",
        Diameter: 159, PipeWallThickness: 6, Status: true, Koef: 0.150,
    },
    {
        ID: 7, IDCat: 10, IDType: 100,
        Name: "Труба стальная 219×8 ГОСТ 8732-78",
        Gost: "8732-78", Manufacturer: "ВТЗ", SteelGrade: "09Г2С",
        Diameter: 219, PipeWallThickness: 8, Status: true, Koef: 0.210,
    },
    {
        ID: 8, IDCat: 10, IDType: 100,
        Name: "Труба стальная 273×10 ГОСТ 8732-78",
        Gost: "8732-78", Manufacturer: "ТМК", SteelGrade: "17Г1С",
        Diameter: 273, PipeWallThickness: 10, Status: true, Koef: 0.285,
    },
    {
        ID: 9, IDCat: 10, IDType: 100,
        Name: "Труба стальная 325×12 ГОСТ 8732-78",
        Gost: "8732-78", Manufacturer: "ЧТПЗ", SteelGrade: "20",
        Diameter: 325, PipeWallThickness: 12, Status: true, Koef: 0.380,
    },
];

type Unit = "m" | "t";

export default function CatalogPageMain() {
    const addItem = useCartStore((s) => s.addItem);

    // локальные состояния per-card: единица и количество
    const [state, setState] = useState<Record<string | number, { unit: Unit; qty: number }>>({});

    const ensure = (id: string | number) => {
        const s = state[id];
        return s ?? { unit: "m" as Unit, qty: 6 };
    };

    const setUnit = (id: string | number, unit: Unit) =>
        setState((prev) => ({ ...prev, [id]: { ...ensure(id), unit } }));

    const setQty = (id: string | number, qty: number) =>
        setState((prev) => ({ ...prev, [id]: { ...ensure(id), qty: Math.max(0, qty) } }));

    const inc = (id: string | number) => {
        const { unit, qty } = ensure(id);
        setQty(id, qty + (unit === "m" ? 1 : 0.01));
    };

    const dec = (id: string | number) => {
        const { unit, qty } = ensure(id);
        setQty(id, Math.max(0, qty - (unit === "m" ? 1 : 0.01)));
    };

    const quick = (id: string | number, v: number, unit: Unit) => {
        setUnit(id, unit);
        setQty(id, v);
    };

    const handleAdd = (p: Nomenclature) => {
        const { unit, qty } = ensure(p.ID);
        // stockId пока заглушка = 1; addItem суммирует, если товар уже в корзине
        addItem(p, qty, unit, 1);
    };

    return (
        <div className="catalog-container">
            <main className="catalog">
                <h2 className="checkout__title">Каталог товаров</h2>
                {/* Панель фильтров (пока заглушка) */}
                <section className="catalog__filters">
                    <div className="filters__row">
                        <select className="filters__control"><option>Склад</option></select>
                        <select className="filters__control"><option>Вид продукции</option></select>
                        <select className="filters__control"><option>Диаметр</option></select>
                        <select className="filters__control"><option>Толщина стенки</option></select>
                    </div>
                    <div className="filters__row">
                        <select className="filters__control"><option>ГОСТ</option></select>
                        <select className="filters__control"><option>Марка стали</option></select>
                        <select className="filters__control"><option>Технология производства</option></select>
                        <select className="filters__control"><option>Производитель</option></select>
                    </div>
                    <div className="filters__actions">
                        <button className="btn btn--primary" type="button">Найти</button>
                        <button className="btn btn--ghost" type="button">Сбросить</button>
                    </div>
                </section>

                {/* Карточки */}
                <section className="catalog__grid">
                    {mockProducts.map((p) => {
                        const s = ensure(p.ID);
                        return (
                            <article className="productCard" key={p.ID}>
                                <div className="productCard__title">{p.Name}</div>

                                <div className="productCard__meta">
                                    <span>Ø {p.Diameter}</span>
                                    <span>стенка {p.PipeWallThickness}</span>
                                    <span>ГОСТ {p.Gost}</span>
                                </div>

                                <div className="productCard__priceRow">
                                    <div className="productCard__price">
                                        <div className="productCard__priceLabel">Цена, т</div>
                                        <div className="productCard__priceValue">—</div>
                                    </div>
                                    <div className="productCard__price">
                                        <div className="productCard__priceLabel">Цена, м</div>
                                        <div className="productCard__priceValue">—</div>
                                    </div>
                                </div>

                                {/* Варианты покупки */}
                                <div className="productCard__purchase">
                                    <div className="seg">
                                        <button
                                            className={`seg__btn ${s.unit === "m" ? "is-active" : ""}`}
                                            onClick={() => setUnit(p.ID, "m")}
                                            type="button"
                                        >
                                            м
                                        </button>
                                        <button
                                            className={`seg__btn ${s.unit === "t" ? "is-active" : ""}`}
                                            onClick={() => setUnit(p.ID, "t")}
                                            type="button"
                                        >
                                            т
                                        </button>
                                    </div>

                                    <div className="qty">
                                        <button className="qty__btn" onClick={() => dec(p.ID)} type="button">−</button>
                                        <input
                                            className="qty__input"
                                            value={s.qty}
                                            onChange={(e) => {
                                                const v = Number(e.target.value.replace(",", "."));
                                                if (!Number.isNaN(v)) setQty(p.ID, v);
                                            }}
                                        />
                                        <button className="qty__btn" onClick={() => inc(p.ID)} type="button">+</button>
                                    </div>

                                    <div className="quick">
                                        <button className="chip" type="button" onClick={() => quick(p.ID, 6, "m")}>6 м</button>
                                        <button className="chip" type="button" onClick={() => quick(p.ID, 12, "m")}>12 м</button>
                                        <button className="chip" type="button" onClick={() => quick(p.ID, 0.24, "t")}>0.24 т</button>
                                    </div>
                                </div>

                                {/* Заполнитель, чтобы кнопка всегда была внизу */}
                                <div className="productCard__spacer" />

                                <button className="btn btn--primary btn--full" type="button" onClick={() => handleAdd(p)}>
                                    В корзину
                                </button>
                            </article>
                        );
                    })}
                </section>
            </main>
        </div>
    );
}
