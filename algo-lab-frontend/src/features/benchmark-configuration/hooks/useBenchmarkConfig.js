import { useMemo, useState } from "react";

const DEFAULT_CONFIG = {
    startN: 1,
    endN: 10000,
    step: 10,
    startM: null,
    endM: null,
    forceRecalculate: false,
};

export default function useBenchmarkConfig() {
    const [selected, setSelected] = useState([]);

    const addAlgorithm = (algorithm) => {
        setSelected((current) => {
            const exists = current.some((item) => item.algorithmId === algorithm.id);
            if (exists) {
                return current;
            }
            return [
                ...current,
                {
                    algorithmId: algorithm.id,
                    inputArity: algorithm.inputArity,
                    ...DEFAULT_CONFIG,
                },
            ];
        });
    };

    const removeAlgorithm = (algorithmId) => {
        setSelected((current) => current.filter((item) => item.algorithmId !== algorithmId));
    };

    const updateConfig = (algorithmId, field, value) => {
        setSelected((current) =>
            current.map((item) => {
                if (item.algorithmId !== algorithmId) {
                    return item;
                }
                return {
                    ...item,
                    [field]: value,
                };
            })
        );
    };

    const clear = () => {
        setSelected([]);
    };

    const validationErrors = useMemo(() => {
        const result = {};
        selected.forEach((config) => {
            const errors = {};
            
            if (!Number.isInteger(config.startN) || config.startN < 1) {
                errors.startN = "Начальное N должно быть не меньше 1.";
            }
            if (!Number.isInteger(config.endN) || config.endN < 1) {
                errors.endN = "Конечное N должно быть не меньше 1.";
            }
            if (
                Number.isInteger(config.startN) &&
                Number.isInteger(config.endN) &&
                config.endN < config.startN
            ) {
                errors.endN = "Конечное N должно быть больше или равно начальному N.";
            }
            if (!Number.isInteger(config.step) || config.step < 1) {
                errors.step = "Шаг должен быть не меньше 1.";
            }

            // Валидация для 2D алгоритмов
            if (config.inputArity === 2) {
                if (!Number.isInteger(config.startM) || config.startM < 1) {
                    errors.startM = "Начальное M должно быть не меньше 1.";
                }
                if (!Number.isInteger(config.endM) || config.endM < 1) {
                    errors.endM = "Конечное M должно быть не меньше 1.";
                }
                if (
                    Number.isInteger(config.startM) &&
                    Number.isInteger(config.endM) &&
                    config.endM < config.startM
                ) {
                    errors.endM = "Конечное M должно быть больше или равно начальному M.";
                }
            }

            result[config.algorithmId] = errors;
        });
        return result;
    }, [selected]);

    const isValid =
        selected.length > 0 &&
        selected.every((config) => {
            const errors = validationErrors[config.algorithmId];
            return !errors || Object.keys(errors).length === 0;
        });

    return {
        selected,
        addAlgorithm,
        removeAlgorithm,
        updateConfig,
        clear,
        validationErrors,
        isValid,
    };
}