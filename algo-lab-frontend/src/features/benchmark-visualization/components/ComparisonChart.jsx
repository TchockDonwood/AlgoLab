import { useEffect, useRef } from "react";
import Plotly from "plotly.js-dist-min";

export default function ComparisonChart({ series = [], title = "Сравнение алгоритмов" }) {
    const chartRef = useRef(null);

    useEffect(() => {
        if (!chartRef.current) return;

        const hasSteps = series.some((s) =>
            s.points?.some((p) => Number(p.steps) > 0)
        );

        const data = series
            .filter((item) => item?.points?.length > 0)
            .map((item) => {
                const isStepBased = item.points.some(
                    (p) => Number(p.steps) > 0
                );

                return {
                    x: item.points.map((point) => point.n),
                    y: item.points.map((point) =>
                        isStepBased
                            ? Math.round(Number(point.steps) || 0)
                            : (Number(point.executionTimeMs) || 0)
                    ),
                    type: "scatter",
                    mode: "lines",
                    name: isStepBased
                        ? `${item.algorithmName} (Шаги)`
                        : `${item.algorithmName} (Время)`,
                    marker: { size: 6 },
                    line: { width: 2 },
                    hovertemplate: isStepBased
                        ? `<b>${item.algorithmName}</b><br>N = %{x}<br>Шаги = %{y}<extra></extra>`
                        : `<b>${item.algorithmName}</b><br>N = %{x}<br>Время = %{y:,.4f} мс<extra></extra>`,
                };
            });

        const layout = {
            title: { text: title },
            xaxis: { title: { text: "Количество N" }, zeroline: false },
            yaxis: {
                title: { text: "Значение (Время / Шаги)" },
                zeroline: false,
                tickformat: hasSteps ? ".0f" : "",
                exponentformat: hasSteps ? "none" : "e",
            },
            hovermode: "x unified",
            legend: { orientation: "h", y: -0.2 },
            margin: { l: 80, r: 30, t: 70, b: 100 },
            paper_bgcolor: "transparent",
            plot_bgcolor: "transparent",
        };

        Plotly.react(chartRef.current, data, layout, { responsive: true, displaylogo: false });

        return () => {
            if (chartRef.current) Plotly.purge(chartRef.current);
        };
    }, [series, title]);

    return <div ref={chartRef} className="plot-wrapper" style={{ width: "100%", height: "520px" }} />;
}