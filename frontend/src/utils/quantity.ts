export function roundToStepUp(value: number, step?: number) {
    if (!step || step <= 0) return value;
    const k = Math.ceil(value / step);
    return k * step;
}

/** Перевод по коэффициенту t/m */
export function mToT(m: number, koef?: number) {
    return koef ? m * koef : 0;
}
export function tToM(t: number, koef?: number) {
    return koef ? t / koef : 0;
}
