import { Link, useLocation } from "react-router-dom";
import { useState } from "react";
import { useCartStore } from "../../store/useCartStore";
import "./Header.css";

const Header = () => {
    const location = useLocation();
    const [open, setOpen] = useState(false);

    const count = useCartStore((s) => s.items.length);

    const closeMenu = () => setOpen(false);

    return (
        <header className="header">
            <div className="header__inner">
                {/* Лого */}
                <Link to="/catalog" className="header__brand" onClick={closeMenu}>
                    <span className="header__logo">TMK</span>
                    <span className="header__tag">mini app</span>
                </Link>

                {/* Навигация (desktop) */}
                <nav className="header__nav">
                    <Link
                        to="/catalog"
                        className={`header__navLink ${location.pathname === "/catalog" ? "active" : ""}`}
                    >
                        Каталог
                    </Link>
                    <Link
                        to="/cart"
                        className={`header__navLink ${location.pathname === "/cart" ? "active" : ""}`}
                    >
                        Корзина
                        {count > 0 && <span className="header__cartCount">{count}</span>}
                    </Link>
                    <Link
                        to="/checkout"
                        className={`header__navLink ${location.pathname === "/checkout" ? "active" : ""}`}
                    >
                        Заказ
                    </Link>
                </nav>

                {/* Бургер (mobile ≤480px) */}
                <button
                    className={`header__burger ${open ? "is-open" : ""}`}
                    aria-label="Открыть меню"
                    aria-expanded={open}
                    onClick={() => setOpen(v => !v)}
                >
                    <span />
                    <span />
                    <span />
                </button>
            </div>

            {/* Выпадающее мобильное меню */}
            <nav className={`header__mobile ${open ? "open" : ""}`} role="menu">
                <Link
                    to="/catalog"
                    className={`header__mobileLink ${location.pathname === "/catalog" ? "active" : ""}`}
                    onClick={closeMenu}
                    role="menuitem"
                >
                    Каталог
                </Link>
                <Link
                    to="/cart"
                    className={`header__mobileLink ${location.pathname === "/cart" ? "active" : ""}`}
                    onClick={closeMenu}
                    role="menuitem"
                >
                    Корзина
                    {count > 0 && <span className="header__cartCount">{count}</span>}
                </Link>
                <Link
                    to="/checkout"
                    className={`header__mobileLink ${location.pathname === "/checkout" ? "active" : ""}`}
                    onClick={closeMenu}
                    role="menuitem"
                >
                    Заказ
                </Link>
            </nav>
        </header>
    );
};

export default Header;
