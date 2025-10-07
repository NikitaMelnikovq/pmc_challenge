import { create } from "zustand";
import type { CartItem, QtyUnit } from "../types/cart";
import type { Nomenclature } from "../types/product";

type State = { items: CartItem[] };
type Actions = {
    addItem: (product: Nomenclature, qty: number, unit: QtyUnit, stockId: string | number) => void;
    updateQty: (id: string | number, qty: number, unit: QtyUnit) => void;
    removeItem: (id: string | number) => void;
    clear: () => void;
};

export const useCartStore = create<State & Actions>((set, get) => ({
    items: [],

    addItem: (product, qty, unit, stockId) => {
        const exists = get().items.find(i => i.id === product.ID);
        if (exists) {
            set(s => ({
                items: s.items.map(i => i.id === product.ID ? { ...i, qty: i.qty + qty, unit } : i)
            }));
        } else {
            set(s => ({
                items: [...s.items, { id: product.ID, product, qty, unit, stockId }]
            }));
        }
    },

    updateQty: (id, qty, unit) => {
        set(s => ({
            items: s.items.map(i => (i.id === id ? { ...i, qty, unit } : i)),
        }));
    },

    removeItem: (id) => set(s => ({ items: s.items.filter(i => i.id !== id) })),

    clear: () => set({ items: [] }),
}));
