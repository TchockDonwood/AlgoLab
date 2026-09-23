const statusLabels = {
    pending: "Ожидает",
    running: "Выполняется",
    completed: "Завершен",
    cancelled: "Отменен",
    failed: "Ошибка",
};

export default function StatusBadge({ status }) {
    const normalizedStatus = String(status ?? "").toLowerCase();
    const label = statusLabels[normalizedStatus] ?? status ?? "Неизвестно";

    return (
        <span className={`status-badge status-badge--${normalizedStatus || "unknown"}`}>
            {label}
        </span>
    );
}