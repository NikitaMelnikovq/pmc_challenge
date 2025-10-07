declare global {
    interface Window {
        Telegram: {
            WebApp: {
                initData: string;
                initDataUnsafe: any;
                colorScheme: 'light' | 'dark';
                themeParams: Record<string, string>;
                isExpanded: boolean;
                expand: () => void;
                ready: () => void;
                MainButton: {
                    text: string;
                    isVisible: boolean;
                    isActive: boolean;
                    show: () => void;
                    hide: () => void;
                    enable: () => void;
                    disable: () => void;
                    setText: (text: string) => void;
                    onClick: (cb: () => void) => void;
                };
                BackButton: { show: () => void; hide: () => void; onClick: (cb: () => void) => void };
                onEvent: (e: string, cb: (...args: any[]) => void) => void;
                offEvent: (e: string, cb: (...args: any[]) => void) => void;
            };
        };
    }
}

export { };
