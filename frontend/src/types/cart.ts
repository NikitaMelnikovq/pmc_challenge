import type { ID, Nomenclature } from "./product";

export type QtyUnit = "m" | "t";

export interface CartItem {
    id: ID;                 // Nomenclature.ID
    product: Nomenclature;
    qty: number;            // значение в выбранной единице
    unit: QtyUnit;          // "m" или "t"
    stockId: ID;            // склад
}
