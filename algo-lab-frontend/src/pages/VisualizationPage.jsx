import { useEffect, useMemo, useState } from "react";
import { useQuery } from "@tanstack/react-query";
import { getComparison } from "../api/benchmarks";
import { useBenchmarkHistory } from "../features/benchmark-history/hooks/useBenchmarkHistory";
import useBenchmarkDetails from "../features/benchmark-visualization/hooks/useBenchmarkDetails";
import ComplexityChart from "../features/benchmark-visualization/components/ComplexityChart";
import SurfaceChart from "../features/benchmark-visualization/components/SurfaceChart";
import ComparisonChart from "../features/benchmark-visualization/components/ComparisonChart";
import RunDetailsTable from "../features/benchmark-visualization/components/RunDetailsTable";
import StatusBadge from "../components/StatusBadge/StatusBadge";
import Button from "../components/Button/Button";

export default function VisualizationPage({ sessionId, onClose }) {
    const {
        data: details,
        isLoading: detailsLoading,
        isError: detailsError,
    } = useBenchmarkDetails(sessionId);

    const { data: history = [], isLoading: historyLoading } = useBenchmarkHistory();
    const [selectedComparisonIds, setSelectedComparisonIds] = useState([]);

    useEffect(() => {
        if (sessionId !== null && sessionId !== undefined) {
            setSelectedComparisonIds([sessionId]);
        }
    }, [sessionId]);

    const sessionMeta = useMemo(() => {
        return history.find((session) => session.id === sessionId);
    }, [history, sessionId]);

    const availableComparisonSessions = useMemo(() => {
        const completed = history.filter((session) => session.status === "Completed");
        const map = new Map();
        completed.forEach((session) => {
            const existing = map.get(session.algorithmId);
            if (!existing) {
                map.set(session.algorithmId, session);
                return;
            }
            const existingDate = new Date(existing.finishedAt || existing.createdAt);
            const currentDate = new Date(session.finishedAt || session.createdAt);
            if (currentDate > existingDate) {
                map.set(session.algorithmId, session);
            }
        });
        return Array.from(map.values());
    }, [history]);

    const comparisonIds = useMemo(() => {
        const ids = [...selectedComparisonIds];
        if (sessionId !== null && sessionId !== undefined && !ids.includes(sessionId)) {
            ids.unshift(sessionId);
        }
        return ids;
    }, [selectedComparisonIds, sessionId]);

    const { data: comparisonData = [], isLoading: comparisonLoading } = useQuery({
        queryKey: ["benchmark-comparison", comparisonIds],
        queryFn: () => getComparison(comparisonIds),
        enabled: comparisonIds.length > 0,
    });

    const comparisonSeries = comparisonData
        .filter((item) => item?.points?.length > 0)
        .map((item) => ({
            algorithmName: item.algorithmName || "Алгоритм",
            points: item.points,
        }));

    const toggleComparison = (id) => {
        setSelectedComparisonIds((current) => {
            if (current.includes(id)) {
                if (id === sessionId) return current;
                return current.filter((item) => item !== id);
            }
            return [...current, id];
        });
    };

    if (detailsLoading) {
        return (
            <section className="panel">
                <div className="empty-state">Загрузка бенчмарка...</div>
            </section>
        );
    }

    if (detailsError || !details) {
        return (
            <section className="panel">
                <div className="error-state">Не удалось загрузить данные бенчмарка.</div>
                <Button variant="secondary" onClick={onClose}>Назад</Button>
            </section>
        );
    }

    // Определяем тип метрики для заголовка
    const isStepBased = details.points?.some(
        (p) => p.steps !== null && p.steps !== undefined && p.steps > 0
    );

    const startN = sessionMeta?.startN;
    const endN = sessionMeta?.endN;
    const step = sessionMeta?.step;
    const is2D = details.inputArity === 2;

    return (
        <section className="visualization-page">
            <div className="visualization-header">
                <div>
                    <div className="eyebrow">Детали бенчмарка</div>
                    <h2>{details.algorithmName}</h2>
                    <div className="visualization-meta">
                        <span>N: {startN ?? "—"} — {endN ?? "—"}</span>
                        <span>Шаг: {step ?? "—"}</span>
                        <StatusBadge status={details.status} />
                    </div>
                </div>
                <Button variant="secondary" onClick={onClose}>
                    Закрыть
                </Button>
            </div>

            <div className="panel">
                <h3 className="section-title">
                    {isStepBased ? "Зависимость количества шагов от N" : "Время выполнения"}
                </h3>
                {details.points?.length ? (
                    is2D ? (
                        <SurfaceChart points={details.points} algorithmName={details.algorithmName} />
                    ) : (
                        <ComplexityChart
                            points={details.points}
                            algorithmName={details.algorithmName}
                            approximationModel={details.approximationModel}
                            approximationPoints={details.approximationPoints}
                        />
                    )
                ) : (
                    <div className="empty-state">Точки замеров пока отсутствуют.</div>
                )}
            </div>

            <div className="panel">
                <div className="section-header">
                    <div>
                        <h3 className="section-title">Сравнение алгоритмов</h3>
                        <p className="section-description">
                            Выберите завершенные сессии бенчмарка для отображения на одном графике.
                        </p>
                    </div>
                </div>
                {historyLoading ? (
                    <div className="empty-state">Загрузка вариантов сравнения...</div>
                ) : (
                    <div className="comparison-selector">
                        {availableComparisonSessions.map((session) => {
                            const checked = selectedComparisonIds.includes(session.id);
                            return (
                                <label key={session.id} className="comparison-option">
                                    <input
                                        type="checkbox"
                                        checked={checked}
                                        disabled={session.id === sessionId}
                                        onChange={() => toggleComparison(session.id)}
                                    />
                                    <span className="comparison-option-name">{session.algorithmName}</span>
                                    <span className="comparison-option-meta">
                                        N={session.startN}–{session.endN}, шаг={session.step}
                                    </span>
                                </label>
                            );
                        })}
                        {!availableComparisonSessions.length && (
                            <div className="empty-state">Нет завершенных сессий для сравнения.</div>
                        )}
                    </div>
                )}
            </div>

            <div className="panel">
                <h3 className="section-title">Сравнение</h3>
                {comparisonLoading ? (
                    <div className="empty-state">Загрузка сравнения...</div>
                ) : (
                    <ComparisonChart series={comparisonSeries} title="Сравнение алгоритмов" />
                )}
            </div>

            <div className="panel">
                <div className="section-header">
                    <div>
                        <h3 className="section-title">Замеры</h3>
                        <p className="section-description">
                            {details.points?.length || 0} точек замера.
                        </p>
                    </div>
                </div>
                <RunDetailsTable points={details.points || []} />
            </div>
        </section>
    );
}