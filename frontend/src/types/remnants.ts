import type { ID } from "./product";

export interface Remnant {
    ID: ID;           // Nomenclature.ID
    IDStock: ID;
    InStockT?: number;
    InStockM?: number;
    AvgTubeLength?: number; // шаг длины (м)
    AvgTubeWeight?: number; // шаг веса (т)
}
