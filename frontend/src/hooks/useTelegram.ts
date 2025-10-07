import { useEffect, useMemo, useState } from "react";

export function useTelegram() {
    const tg = useMemo(() => window?.Telegram?.WebApp, []);
    const [scheme, setScheme] = useState<"light" | "dark">(tg?.colorScheme ?? "light");

    useEffect(() => {
        if (!tg) return;
        tg.ready();      // сообщает Telegram, что страница загрузилась
        tg.expand();     // разворачивает MiniApp на весь экран

        const handleTheme = () => setScheme(tg.colorScheme);
        tg.onEvent("themeChanged", handleTheme);
        return () => tg.offEvent("themeChanged", handleTheme);
    }, [tg]);

    return { tg, scheme, themeParams: tg?.themeParams ?? {} };
}
