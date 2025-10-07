import { api } from "./api";
import type { Nomenclature } from "../types/product";
import type { Price } from "../types/price";
import type { Remnant } from "../types/remnants";
import type { Stock } from "../types/stock";

export async function fetchNomenclature(params: Record<string, any> = {}): Promise<Nomenclature[]> {
    const { data } = await api.get<Nomenclature[]>("/nomenclature", { params });
    return data;
}

export async function fetchPrices(params: Record<string, any> = {}): Promise<Price[]> {
    const { data } = await api.get<Price[]>("/prices", { params });
    return data;
}

export async function fetchRemnants(params: Record<string, any> = {}): Promise<Remnant[]> {
    const { data } = await api.get<Remnant[]>("/remnants", { params });
    return data;
}

export async function fetchStocks(): Promise<Stock[]> {
    const { data } = await api.get<Stock[]>("/stocks");
    return data;
}
