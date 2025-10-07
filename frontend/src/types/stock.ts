import type { ID } from "./product";

export interface Stock {
    IDStock: ID;
    Stock: string;       // город
    StockName?: string;
}
