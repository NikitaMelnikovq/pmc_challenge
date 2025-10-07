export type ID = number | string;

export interface TypeItem {
    IDType: ID;
    Type: string;
}

export interface Nomenclature {
    ID: ID;
    IDCat: ID;
    IDType: ID;
    IDTypeNew?: string;
    ProductionType?: string;
    IDFunctionType?: ID;
    Name: string;
    Gost: string;           // "ГОСТ1, ГОСТ2"
    FormOfLength?: string;
    Manufacturer?: string;
    SteelGrade?: string;
    Diameter?: number;
    ProfileSize2?: number;
    PipeWallThickness?: number;
    Status?: boolean;       // наличие
    Koef?: number;          // t/m
}
