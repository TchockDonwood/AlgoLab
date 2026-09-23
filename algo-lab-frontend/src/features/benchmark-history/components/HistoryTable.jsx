import { useMutation, useQueryClient } from "@tanstack/react-query";
import Table from "../../../components/Table/Table";
import Button from "../../../components/Button/Button";
import StatusBadge from "../../../components/StatusBadge/StatusBadge";
import { cancelBenchmark } from "../../../api/benchmarks";
import { useBenchmarkHistory } from "../hooks/useBenchmarkHistory";

function formatDate(value) {
    if (!value) return "—";
    return new Intl.DateTimeFormat("ru-RU", {
        dateStyle: "short",
        timeStyle: "medium",
    }).format(new Date(value));
}

export default function HistoryTable({ onSelectSession }) {
    const queryClient = useQueryClient();
    const {
        data: sessions = [],
        isLoading,
        isError,
        error,
    } = useBenchmarkHistory();

    const cancelMutation = useMutation({
        mutationFn: cancelBenchmark,
        onSuccess: async () => {
            await queryClient.invalidateQueries({ queryKey: ["benchmarks"] });
        },
    });

    const handleCancel = (id) => cancelMutation.mutate(id);

    if (isLoading) return <div className="empty-state">Загрузка истории...</div>;
    if (isError) return <div className="error-state">Ошибка загрузки: {error?.message}</div>;

    const columns = [
        {
            key: "id",
            label: "ID",
            render: (row) => (row.id ? `${row.id.slice(0, 8)}...` : "—"),
        },
        { key: "algorithmName", label: "Алгоритм" },
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
                const canCancel = row.status === "Pending" || row.status === "Running";
                const canOpen = ["Completed", "Cancelled", "Failed"].includes(row.status);

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
                                loading={cancelMutation.isPending && cancelMutation.variables === row.id}
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

    return (
        <Table
            columns={columns}
            data={sessions}
            rowKey={(row) => row.id}
            onRowClick={(row) => {
                if (["Completed", "Cancelled", "Failed"].includes(row.status)) {
                    onSelectSession(row.id);
                }
            }}
            emptyMessage="История замеров пуста."
        />
    );
}