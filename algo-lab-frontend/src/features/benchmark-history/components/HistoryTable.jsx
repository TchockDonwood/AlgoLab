import { useMemo, useState } from "react";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import Table from "../../../components/Table/Table";
import Button from "../../../components/Button/Button";
import StatusBadge from "../../../components/StatusBadge/StatusBadge";
import { cancelBenchmark } from "../../../api/benchmarks";
import { useBenchmarkHistory } from "../hooks/useBenchmarkHistory";

const RECENT_LIMIT = 5;

function formatDate(value) {
    if (!value) return "—";
    return new Intl.DateTimeFormat("ru-RU", {
        dateStyle: "short",
        timeStyle: "medium",
    }).format(new Date(value));
}

function formatBatchTime(value) {
    if (!value) return "—";
    return new Intl.DateTimeFormat("ru-RU", {
        hour: "2-digit",
        minute: "2-digit",
    }).format(new Date(value));
}

function formatDayLabel(value) {
    if (!value || value === "unknown") return "Дата неизвестна";

    const date = new Date(value);
    const today = new Date();
    const yesterday = new Date();
    yesterday.setDate(today.getDate() - 1);

    if (date.toDateString() === today.toDateString()) return "Сегодня";
    if (date.toDateString() === yesterday.toDateString()) return "Вчера";

    return new Intl.DateTimeFormat("ru-RU", {
        day: "numeric",
        month: "long",
        year: "numeric",
    }).format(date);
}

function getDayKey(value) {
    if (!value) return "unknown";
    return new Date(value).toDateString();
}

// Ключ партии запусков: одна и та же дата + час + минута + секунда
function getBatchKey(value) {
    if (!value) return "unknown";
    const d = new Date(value);
    return [
        d.getFullYear(),
        d.getMonth(),
        d.getDate(),
        d.getHours(),
        d.getMinutes(),
        d.getSeconds(),
    ].join("-");
}

// Разбиваем партии внутри дня на визуальные блоки:
// - «batch» — если в партии больше одного запуска (с заголовком)
// - «loose» — соседние одиночные запуски объединяем в общую таблицу
function buildBlocks(batches) {
    const blocks = [];
    let loose = [];

    const flushLoose = () => {
        if (loose.length) {
            blocks.push({ type: "loose", sessions: loose });
            loose = [];
        }
    };

    batches.forEach((batch) => {
        if (batch.sessions.length > 1) {
            flushLoose();
            blocks.push({ type: "batch", batch });
        } else {
            loose.push(...batch.sessions);
        }
    });

    flushLoose();
    return blocks;
}

export default function HistoryTable({ onSelectSession }) {
    const queryClient = useQueryClient();
    const {
        data: sessions = [],
        isLoading,
        isError,
        error,
    } = useBenchmarkHistory();

    const [showAll, setShowAll] = useState(false);
    const [algorithmFilter, setAlgorithmFilter] = useState("");

    const cancelMutation = useMutation({
        mutationFn: cancelBenchmark,
        onSuccess: async () => {
            await queryClient.invalidateQueries({ queryKey: ["benchmarks"] });
        },
    });

    const handleCancel = (id) => cancelMutation.mutate(id);

    // Уникальные имена алгоритмов для select
    const algorithmNames = useMemo(() => {
        const set = new Set();
        sessions.forEach((session) => {
            if (session.algorithmName) set.add(session.algorithmName);
        });
        return Array.from(set).sort((a, b) => a.localeCompare(b, "ru"));
    }, [sessions]);

    // Фильтрация по точному имени алгоритма
    const filteredSessions = useMemo(() => {
        if (!algorithmFilter) return sessions;
        return sessions.filter(
            (session) => session.algorithmName === algorithmFilter
        );
    }, [sessions, algorithmFilter]);

    // Ограничение 5 последними запусками (если не раскрыто)
    const visibleSessions = useMemo(() => {
        return showAll
            ? filteredSessions
            : filteredSessions.slice(0, RECENT_LIMIT);
    }, [filteredSessions, showAll]);

    // Количество запусков по дням — считается по всем отфильтрованным,
    // а не только по видимым (иначе цифра «прыгает» после раскрытия)
    const dayCounts = useMemo(() => {
        const counts = new Map();
        filteredSessions.forEach((session) => {
            const key = getDayKey(session.createdAt);
            counts.set(key, (counts.get(key) || 0) + 1);
        });
        return counts;
    }, [filteredSessions]);

    // Группировка по дням → по партиям запусков
    const groupedSessions = useMemo(() => {
        const byDay = new Map();

        visibleSessions.forEach((session) => {
            const dayKey = getDayKey(session.createdAt);

            if (!byDay.has(dayKey)) {
                byDay.set(dayKey, new Map());
            }

            const batches = byDay.get(dayKey);
            const batchKey = getBatchKey(session.createdAt);

            if (!batches.has(batchKey)) {
                batches.set(batchKey, []);
            }
            batches.get(batchKey).push(session);
        });

        return Array.from(byDay.entries()).map(([dayKey, batchesMap]) => ({
            dayKey,
            batches: Array.from(batchesMap.entries()).map(
                ([batchKey, sessions]) => ({ batchKey, sessions })
            ),
        }));
    }, [visibleSessions]);

    if (isLoading) return <div className="empty-state">Загрузка истории...</div>;
    if (isError) return <div className="error-state">Ошибка загрузки: {error?.message}</div>;

    const columns = [
        {
            key: "id",
            label: "ID",
            render: (row) => (row.id ? `${row.id.slice(0, 8)}...` : "—"),
        },
        {
            key: "algorithmName",
            label: "Алгоритм",
            render: (row) => (
                <span
                    className="cell-truncate"
                    title={row.algorithmName || ""}
                >
                    {row.algorithmName || "—"}
                </span>
            ),
        },
        { key: "startN", label: "Нач. N" },
        { key: "endN", label: "Кон. N" },
        { key: "step", label: "Шаг" },
        {
            key: "status",
            label: "Статус",
            render: (row) => <StatusBadge status={row.status} />,
        },
        {
            key: "createdAt",
            label: "Создано",
            render: (row) => formatDate(row.createdAt),
        },
        {
            key: "finishedAt",
            label: "Завершено",
            render: (row) => formatDate(row.finishedAt),
        },
        {
            key: "actions",
            label: "Действия",
            render: (row) => {
                const canCancel =
                    row.status === "Pending" || row.status === "Running";
                const canOpen = ["Completed", "Cancelled", "Failed"].includes(
                    row.status
                );

                return (
                    <div className="table-actions">
                        {canOpen && (
                            <Button
                                variant="secondary"
                                size="small"
                                onClick={(e) => {
                                    e.stopPropagation();
                                    onSelectSession(row.id);
                                }}
                            >
                                Открыть
                            </Button>
                        )}
                        {canCancel && (
                            <Button
                                variant="danger"
                                size="small"
                                loading={
                                    cancelMutation.isPending &&
                                    cancelMutation.variables === row.id
                                }
                                onClick={(e) => {
                                    e.stopPropagation();
                                    handleCancel(row.id);
                                }}
                            >
                                Отменить
                            </Button>
                        )}
                    </div>
                );
            },
        },
    ];

    const handleRowClick = (row) => {
        if (["Completed", "Cancelled", "Failed"].includes(row.status)) {
            onSelectSession(row.id);
        }
    };

    return (
        <div className="history">
            <div className="history-toolbar">
                <div className="ui-input-group history-filter-input">
                    <label className="ui-input-label">
                        Фильтр по алгоритму
                    </label>
                    <select
                        className="ui-input history-filter-select"
                        value={algorithmFilter}
                        onChange={(e) => {
                            setAlgorithmFilter(e.target.value);
                            setShowAll(false);
                        }}
                    >
                        <option value="">Все алгоритмы</option>
                        {algorithmNames.map((name) => (
                            <option key={name} value={name}>
                                {name}
                            </option>
                        ))}
                    </select>
                </div>
            </div>

            {!filteredSessions.length ? (
                <div className="empty-state">
                    {sessions.length
                        ? "По заданному фильтру ничего не найдено."
                        : "История замеров пуста."}
                </div>
            ) : (
                <>
                    {groupedSessions.map((dayGroup) => {
                        const blocks = buildBlocks(dayGroup.batches);

                        return (
                            <div
                                key={dayGroup.dayKey}
                                className="history-day-group"
                            >
                                <div className="history-day-header">
                                    <span>
                                        {formatDayLabel(dayGroup.dayKey)}
                                    </span>
                                    <span className="history-day-count">
                                        {dayCounts.get(dayGroup.dayKey) || 0}
                                    </span>
                                </div>

                                {blocks.map((block, index) => {
                                    if (block.type === "batch") {
                                        return (
                                            <div
                                                key={`batch-${block.batch.batchKey}`}
                                                className="history-batch"
                                            >
                                                <div className="history-batch-header">
                                                    <span className="history-batch-time">
                                                        {formatBatchTime(
                                                            block.batch
                                                                .sessions[0]
                                                                .createdAt
                                                        )}
                                                    </span>
                                                    <span className="history-batch-count">
                                                        {
                                                            block.batch.sessions
                                                                .length
                                                        }{" "}
                                                        запусков
                                                    </span>
                                                </div>
                                                <Table
                                                    columns={columns}
                                                    data={block.batch.sessions}
                                                    rowKey={(row) => row.id}
                                                    onRowClick={handleRowClick}
                                                    emptyMessage="Нет записей."
                                                />
                                            </div>
                                        );
                                    }

                                    return (
                                        <Table
                                            key={`loose-${index}`}
                                            columns={columns}
                                            data={block.sessions}
                                            rowKey={(row) => row.id}
                                            onRowClick={handleRowClick}
                                            emptyMessage="Нет записей."
                                        />
                                    );
                                })}
                            </div>
                        );
                    })}

                    {filteredSessions.length > RECENT_LIMIT && (
                        <div className="history-expand">
                            <Button
                                variant="ghost"
                                size="medium"
                                onClick={() => setShowAll((v) => !v)}
                            >
                                {showAll
                                    ? "Свернуть"
                                    : `Показать все (${filteredSessions.length})`}
                            </Button>
                        </div>
                    )}
                </>
            )}
        </div>
    );
}   