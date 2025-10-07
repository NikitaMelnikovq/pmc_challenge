import { useEffect } from "react";
import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";
import { useTelegram } from "./hooks/useTelegram";
import CatalogPage from "./pages/CatalogPage.tsx";
import CartPage from "./pages/CartPage.tsx";
import CheckoutPage from "./pages/CheckoutPage.tsx";

function App() {
  const { tg } = useTelegram();

  useEffect(() => {
    if (tg) {
      console.log("✅ Telegram WebApp обнаружен!");
      console.log("Пользователь:", tg.initDataUnsafe?.user);
      tg.MainButton.setText("Telegram работает ✅");
      tg.MainButton.show();
    } else {
      console.warn("⚠️ Telegram WebApp не найден — открой страницу через Telegram");
    }
  }, [tg]);

  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Navigate to="/catalog" />} />
        <Route path="/catalog" element={<CatalogPage />} />
        <Route path="/cart" element={<CartPage />} />
        <Route path="/checkout" element={<CheckoutPage />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
