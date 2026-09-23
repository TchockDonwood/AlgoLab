import Table from "../../../components/Table/Table";

function formatExecutionTime(value) {
    if (value === null || value === undefined) return "—";
    return Number(value).toFixed(6);
}

function formatSteps(value) {
    if (value === null || value === undefined) return "—";
    return Number(value).toLocaleString("ru-RU");
}

export default function RunDetailsTable({ points = [] }) {
    // Проверяем, есть ли вообще шаги в данных
    const hasSteps = points.some(p => p.steps !== null && p.steps !== undefined);
    // Проверяем, есть ли время
    const hasTime = points.some(p => p.executionTimeMs !== null && p.executionTimeMs !== undefined && p.executionTimeMs > 0);

    const is2D = points.some((p) => p.m !== null && p.m !== undefined);

    const columns = [
        { key: "n", label: "N" },
        ...(is2D
            ? [
                {
                    key: "m",
                    label: "M",
                    render: (row) => row.m ?? "—",
                },
                ]
            : []),
        ...(hasTime ? [{
            key: "executionTimeMs",
            label: "Время (мс)",
            render: (row) => formatExecutionTime(row.executionTimeMs),
        }] : []),
        ...(hasSteps ? [{
            key: "steps",
            label: "Шаги",
            render: (row) => formatSteps(row.steps),
        }] : []),
        {
            key: "fromCache",
            label: "Источник",
            render: (row) => (row.fromCache ? "Кэш" : "Вычислено"),
        },
    ];

    return (
        <Table
            columns={columns}
            data={points}
            rowKey={(row) => `${row.n}-${row.m ?? 0}`}
            emptyMessage="Нет данных о замерах."
        />
    );
}