import { useEffect } from "react";
import { useTelegram } from "./hooks/useTelegram";

function App() {
  const { tg, scheme } = useTelegram();

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
    <div
      style={{
        display: "flex",
        flexDirection: "column",
        alignItems: "center",
        justifyContent: "center",
        height: "100vh",
        background: scheme === "dark" ? "#1e1e1e" : "#f9f9f9",
        color: "#ec6608",
        fontFamily: "Geologica, sans-serif",
      }}
    >
      <h1 style={{ fontSize: "3rem", fontWeight: "bold" }}>TMK</h1>
      <p>Mini App тестовая интеграция</p>
    </div>
  );
}

export default App;
