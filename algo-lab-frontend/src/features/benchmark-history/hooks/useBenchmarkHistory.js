import { useQuery } from "@tanstack/react-query";
import { getHistory } from "../../../api/benchmarks";

export function useBenchmarkHistory() {
  return useQuery({
    queryKey: ["benchmarks"],

    queryFn: getHistory,

    refetchOnWindowFocus: true,

    refetchInterval: (query) => {
      const sessions =
        query.state.data ?? [];

      const hasActive =
        sessions.some(
          (session) =>
            session.status === "Pending" ||
            session.status === "Running"
        );

      return hasActive
        ? 2000
        : false;
    },
  });
}