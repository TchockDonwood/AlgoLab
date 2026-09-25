import { useEffect, useRef, useState } from "react";
import { useQuery } from "@tanstack/react-query";
import { getAlgorithms } from "./api/algorithms";
import AlgorithmList from "./features/benchmark-configuration/components/AlgorithmList";
import AlgorithmConfigForm from "./features/benchmark-configuration/components/AlgorithmConfigForm";
import RunBenchmarkButton from "./features/benchmark-configuration/components/RunBenchmarkButton";
import useBenchmarkConfig from "./features/benchmark-configuration/hooks/useBenchmarkConfig";
import HistoryTable from "./features/benchmark-history/components/HistoryTable";
import VisualizationPage from "./pages/VisualizationPage";
import "./App.css";

export default function App() {
    const [selectedSessionId, setSelectedSessionId] = useState(null);
    const visualizationRef = useRef(null);

    const {
        data: algorithms = [],
        isLoading: algorithmsLoading,
        isError: algorithmsError,
    } = useQuery({
        queryKey: ["algorithms"],
        queryFn: getAlgorithms,
    });

    const {
        selected,
        addAlgorithm,
        removeAlgorithm,
        updateConfig,
        clear,
        validationErrors,
        isValid,
    } = useBenchmarkConfig();

    // Автоскролл к секции графиков при открытии сессии
    useEffect(() => {
        if (selectedSessionId === null) return;

        const timer = setTimeout(() => {
            visualizationRef.current?.scrollIntoView({
                behavior: "smooth",
                block: "start",
            });
        }, 60);

        return () => clearTimeout(timer);
    }, [selectedSessionId]);

    return (
        <div className="app">
            <header className="app-header">
                <div className="app-header-inner">
                    <div>
                        <div className="app-logo">AlgoLab</div>
                        <div className="app-subtitle">Лаборатория бенчмаркинга алгоритмов</div>
                    </div>
                </div>
            </header>

            <main className="app-main">
                <section className="hero">
                    <div className="eyebrow">Бенчмаркинг</div>
                    <h1>Измерение производительности алгоритмов</h1>
                    <p className="hero-description">
                        Настройте алгоритмы, запустите замеры, используйте кэшированные результаты 
                        и сравнивайте время выполнения на графиках.
                    </p>
                </section>

                <section className="panel">
                    <div className="section-header">
                        <div>
                            <h2 className="section-title">1. Выбор алгоритмов</h2>
                            <p className="section-description">
                                Выберите один или несколько алгоритмов для тестирования.
                            </p>
                        </div>
                        <div className="selection-count">
                            Выбрано: {selected.length}
                        </div>
                    </div>

                    {algorithmsError ? (
                        <div className="error-state">Не удалось загрузить список алгоритмов.</div>
                    ) : (
                        <AlgorithmList
                            algorithms={algorithms}
                            selected={selected}
                            onSelect={addAlgorithm}
                            isLoading={algorithmsLoading}
                        />
                    )}
                </section>

                <section className="panel">
                    <div className="section-header">
                        <div>
                            <h2 className="section-title">2. Настройка замеров</h2>
                            <p className="section-description">
                                Каждый выбранный алгоритм будет запущен как отдельная сессия.
                            </p>
                        </div>
                    </div>

                    {!selected.length ? (
                        <div className="empty-state">
                            Выберите алгоритм выше, чтобы настроить параметры запуска.
                        </div>
                    ) : (
                        <div className="config-list">
                            {selected.map((config) => {
                                const algorithm = algorithms.find(
                                    (item) => item.id === config.algorithmId
                                );
                                return (
                                    <AlgorithmConfigForm
                                        key={config.algorithmId}
                                        algorithm={algorithm}
                                        config={config}
                                        errors={validationErrors[config.algorithmId]}
                                        onUpdate={updateConfig}
                                        onRemove={removeAlgorithm}
                                    />
                                );
                            })}
                        </div>
                    )}

                    <div className="run-actions">
                        <RunBenchmarkButton
                            configs={selected}
                            disabled={!isValid}
                            onStarted={clear}
                        />
                    </div>
                </section>

                <section className="panel">
                    <div className="section-header">
                        <div>
                            <h2 className="section-title">3. История замеров</h2>
                            <p className="section-description">
                                Активные сессии обновляются автоматически.
                            </p>
                        </div>
                    </div>
                    <HistoryTable onSelectSession={setSelectedSessionId} />
                </section>

                {selectedSessionId !== null && (
                    <div ref={visualizationRef} className="visualization-anchor">
                        <VisualizationPage
                            sessionId={selectedSessionId}
                            onClose={() => setSelectedSessionId(null)}
                        />
                    </div>
                )}
            </main>
        </div>
    );
}