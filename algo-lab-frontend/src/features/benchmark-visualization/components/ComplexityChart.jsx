import { useEffect, useRef } from "react";
import Plotly from "plotly.js-dist-min";

export default function ComplexityChart({
    points = [],
    algorithmName,
    approximationModel,
    approximationPoints,
}) {
    const chartRef = useRef(null);

    useEffect(() => {
        if (!chartRef.current || !points.length) return;

        const isStepBased = points.some((p) => Number(p.steps) > 0);

        const validPoints = points.filter((p) =>
            isStepBased
                ? p.steps !== null && p.steps !== undefined
                : p.executionTimeMs !== null && p.executionTimeMs !== undefined
        );

        if (validPoints.length === 0) return;

        const x = validPoints.map((point) => point.n);
        const y = validPoints.map((point) =>
            isStepBased
                ? Math.round(Number(point.steps) || 0)
                : (Number(point.executionTimeMs) || 0)
        );

        const yAxisTitle = isStepBased
            ? "Количество шагов"
            : "Время выполнения (мс)";

        const hoverTemplate = isStepBased
            ? "N = %{x}<br>Шаги = %{y}<extra></extra>"
            : "N = %{x}<br>Время = %{y:.6f} мс<extra></extra>";

        const titleText = isStepBased
            ? `${algorithmName || "Алгоритм"} — Зависимость количества шагов от N`
            : `${algorithmName || "Алгоритм"} — Зависимость времени от N`;

        const data = [
            {
                x,
                y,
                type: "scatter",
                mode: "lines",
                name: algorithmName || "Алгоритм",
                marker: { size: 6 },
                line: { width: 2 },
                hovertemplate: hoverTemplate,
            },
        ];

        if (
            !isStepBased &&
            approximationPoints &&
            approximationPoints.length > 0
        ) {
            data.push({
                x: validPoints.map((p) => p.n),
                y: approximationPoints,
                type: "scatter",
                mode: "lines",
                name: `Аппроксимация (${approximationModel})`,
                line: { width: 2, dash: "dash", color: "#ff7f0e" },
                hoverinfo: "skip",
            });
        }

        const layout = {
            title: { text: titleText },
            xaxis: {
                title: { text: "Количество N" },
                zeroline: false,
            },
            yaxis: {
                title: { text: yAxisTitle },
                zeroline: false,
                tickformat: isStepBased ? ".0f" : "",
                exponentformat: isStepBased ? "none" : "e",
            },
            hovermode: "closest",
            margin: { l: 80, r: 30, t: 70, b: 70 },
            paper_bgcolor: "transparent",
            plot_bgcolor: "transparent",
        };

        const config = {
            responsive: true,
            displaylogo: false,
            modeBarButtonsToRemove: ["lasso2d", "select2d"],
        };

        Plotly.react(chartRef.current, data, layout, config);

        return () => {
            if (chartRef.current) Plotly.purge(chartRef.current);
        };
    }, [points, algorithmName, approximationModel, approximationPoints]);

    return (
        <div
            ref={chartRef}
            className="plot-wrapper"
            style={{ width: "100%", height: "520px" }}
        />
    );
}