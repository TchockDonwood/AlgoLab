import Button from "../../../components/Button/Button";
import Input from "../../../components/Input/Input";

export default function AlgorithmConfigForm({
    algorithm,
    config,
    errors = {},
    onUpdate,
    onRemove,
}) {
    if (!algorithm || !config) {
        return null;
    }

    return (
        <div className="config-card">
            <div className="config-card-header">
                <div>
                    <h3 className="config-card-title">{algorithm.name}</h3>
                    {algorithm.code && (
                        <div className="config-card-code">{algorithm.code}</div>
                    )}
                </div>
                <Button variant="danger" size="small" onClick={() => onRemove(algorithm.id)}>
                    Удалить
                </Button>
            </div>
            <div className="config-grid">
                <Input
                    label="Начальное N"
                    type="number"
                    min="1"
                    value={config.startN}
                    onChange={(event) => onUpdate(algorithm.id, "startN", Number(event.target.value))}
                    error={errors.startN}
                />
                <Input
                    label="Конечное N"
                    type="number"
                    min="1"
                    value={config.endN}
                    onChange={(event) => onUpdate(algorithm.id, "endN", Number(event.target.value))}
                    error={errors.endN}
                />
                <Input
                    label="Шаг"
                    type="number"
                    min="1"
                    value={config.step}
                    onChange={(event) => onUpdate(algorithm.id, "step", Number(event.target.value))}
                    error={errors.step}
                />
                
                {algorithm.inputArity === 2 && (
                    <>
                        <Input
                            label="Начальное M"
                            type="number"
                            min="1"
                            value={config.startM || ""}
                            onChange={(event) => onUpdate(algorithm.id, "startM", Number(event.target.value))}
                            error={errors.startM}
                        />
                        <Input
                            label="Конечное M"
                            type="number"
                            min="1"
                            value={config.endM || ""}
                            onChange={(event) => onUpdate(algorithm.id, "endM", Number(event.target.value))}
                            error={errors.endM}
                        />
                    </>
                )}
            </div>
            <label className="checkbox-row">
                <input
                    type="checkbox"
                    checked={config.forceRecalculate}
                    onChange={(event) => onUpdate(algorithm.id, "forceRecalculate", event.target.checked)}
                />
                <span>Принудительный пересчет</span>
            </label>
        </div>
    );
}