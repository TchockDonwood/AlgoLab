import { useQuery } from "@tanstack/react-query";
import { getDetails } from "../../../api/benchmarks";

export default function useBenchmarkDetails(
  id
) {
  return useQuery({
    queryKey: ["benchmark", id],

    queryFn: () =>
      getDetails(id),

    enabled:
      id !== null &&
      id !== undefined,

    refetchOnWindowFocus: true,

    refetchInterval: (query) => {
      const status =
        query.state.data?.status;

      if (
        status === "Pending" ||
        status === "Running"
      ) {
        return 2000;
      }

      return false;
    },
  });
}