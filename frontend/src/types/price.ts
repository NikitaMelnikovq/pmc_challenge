import type { ID } from "./product";

export interface Price {
    ID: ID;           // Nomenclature.ID
    IDStock: ID;      // Stock.IDStock
    PriceT: number;
    PriceLimitT1?: number;
    PriceT1?: number;
    PriceLimitT2?: number;
    PriceT2?: number;
    PriceM: number;
    PriceLimitM1?: number;
    PriceM1?: number;
    PriceLimitM2?: number;
    PriceM2?: number;
    NDS?: number;
}
