import { useEffect, useRef } from "react";
import Plotly from "plotly.js-dist-min";

export default function SurfaceChart({ points = [], algorithmName }) {
  const chartRef = useRef(null);

  // Основной эффект — построение графика
  useEffect(() => {
    if (!chartRef.current || !points.length) return;

    const validPoints = points.filter(
      (p) =>
        p.executionTimeMs !== null &&
        p.executionTimeMs !== undefined &&
        !p.isOutlier
    );

    if (validPoints.length === 0) return;

    const ns = [...new Set(validPoints.map((p) => p.n))].sort((a, b) => a - b);
    const ms = [...new Set(validPoints.map((p) => p.m))].sort((a, b) => a - b);

    const axisTitle = (text) => ({ text, standoff: 12 });

    if (ns.length < 2 || ms.length < 2) {
      const data = [
        {
          x: validPoints.map((p) => p.m),
          y: validPoints.map((p) => p.n),
          z: validPoints.map((p) => p.executionTimeMs),
          type: "scatter3d",
          mode: "markers",
          marker: {
            size: 4,
            color: validPoints.map((p) => p.executionTimeMs),
            colorscale: "Viridis",
            showscale: true,
            colorbar: { title: "Время (мс)" },
          },
        },
      ];

      const layout = {
        title: { text: `${algorithmName} — Время выполнения (точечные данные)` },
        scene: {
          xaxis: { title: axisTitle("Размер M") },
          yaxis: { title: axisTitle("Размер N") },
          zaxis: { title: axisTitle("Время (мс)") },
          aspectmode: "auto",
          camera: { eye: { x: -1.5, y: -1.5, z: 1.5 } },
        },
        margin: { l: 60, r: 30, t: 70, b: 100 },
        paper_bgcolor: "transparent",
        plot_bgcolor: "transparent",
      };

      Plotly.react(chartRef.current, data, layout, {
        responsive: true,
        displaylogo: false,
      });

      return () => {
        if (chartRef.current) Plotly.purge(chartRef.current);
      };
    }

    const allTimes = validPoints.map((p) => p.executionTimeMs);
    const avgTime = allTimes.reduce((sum, t) => sum + t, 0) / allTimes.length;

    const z = ns.map((n) =>
      ms.map((m) => {
        const point = validPoints.find((p) => p.n === n && p.m === m);
        return point ? point.executionTimeMs : avgTime;
      })
    );

    const data = [
      {
        x: ms,
        y: ns,
        z: z,
        type: "surface",
        colorscale: "Viridis",
        contours: {
          z: {
            show: true,
            usecolormap: true,
            highlightcolor: "#ff7f0e",
            project: { z: true },
          },
        },
        colorbar: { title: "Время (мс)", thickness: 15 },
      },
    ];

    const layout = {
      title: { text: `${algorithmName} — Время выполнения` },
      scene: {
        xaxis: { title: axisTitle("Размер M") },
        yaxis: { title: axisTitle("Размер N") },
        zaxis: { title: axisTitle("Время (мс)") },
        aspectmode: "auto",
        camera: { eye: { x: -1.5, y: -1.5, z: 1.5 } },
      },
      margin: { l: 60, r: 30, t: 70, b: 100 },
      paper_bgcolor: "transparent",
      plot_bgcolor: "transparent",
    };

    Plotly.react(chartRef.current, data, layout, {
      responsive: true,
      displaylogo: false,
    });

    return () => {
      if (chartRef.current) Plotly.purge(chartRef.current);
    };
  }, [points, algorithmName]);

  // Дополнительный эффект — форсируем пересчёт размеров после рендера
  useEffect(() => {
    if (!chartRef.current) return;

    const raf = requestAnimationFrame(() => {
      if (chartRef.current) Plotly.Plots.resize(chartRef.current);
    });

    return () => cancelAnimationFrame(raf);
  }, [points, algorithmName]);

  return (
    <div
      ref={chartRef}
      className="plot-wrapper"
      style={{ width: "100%", height: "520px" }}
    />
  );
}