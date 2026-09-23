import { useMutation, useQueryClient } from "@tanstack/react-query";
import Button from "../../../components/Button/Button";
import { startBenchmark } from "../../../api/benchmarks";

export default function RunBenchmarkButton({
    configs = [],
    disabled = false,
    onStarted,
}) {
    const queryClient = useQueryClient();

    const mutation = useMutation({
        mutationFn: async (items) => {
            return Promise.all(items.map((config) => startBenchmark(config)));
        },
        onSuccess: async () => {
            await queryClient.invalidateQueries({ queryKey: ["benchmarks"] });
            onStarted?.();
        },
    });

    const handleClick = () => {
        if (!configs.length || mutation.isPending) return;
        mutation.mutate(configs);
    };

    return (
        <Button
            variant="primary"
            size="large"
            disabled={disabled || !configs.length}
            loading={mutation.isPending}
            onClick={handleClick}
        >
            Запустить замер
        </Button>
    );
}