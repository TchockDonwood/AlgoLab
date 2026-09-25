import Button from "../../../components/Button/Button";

export default function AlgorithmList({
    algorithms = [],
    selected = [],
    onSelect,
    isLoading = false,
}) {
    if (isLoading) {
        return <div className="empty-state">Загрузка алгоритмов...</div>;
    }

    if (!algorithms.length) {
        return <div className="empty-state">Алгоритмы не найдены.</div>;
    }

    const selectedIds = new Set(selected.map((item) => item.algorithmId));

    return (
        <div className="algorithm-list">
            {algorithms.map((algorithm) => {
                const isSelected = selectedIds.has(algorithm.id);
                return (
                    <div
                        key={algorithm.id}
                        className={`algorithm-list-item ${
                            isSelected ? "algorithm-list-item--selected" : ""
                        }`}
                    >
                        <div className="algorithm-list-info">
                            <div className="algorithm-list-name" title={algorithm.name}>
                                {algorithm.name}
                            </div>
                            {algorithm.code && (
                                <div className="algorithm-list-code">{algorithm.code}</div>
                            )}
                            {algorithm.description && (
                                <div className="algorithm-list-description">
                                    {algorithm.description}
                                </div>
                            )}
                        </div>
                        <Button
                            variant={isSelected ? "secondary" : "primary"}
                            size="small"
                            disabled={isSelected}
                            onClick={() => onSelect(algorithm)}
                            className="algorithm-list-action"
                        >
                            {isSelected ? "Выбрано" : "Выбрать"}
                        </Button>
                    </div>
                );
            })}
        </div>
    );
}