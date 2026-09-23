import { api } from "./client";

function normalizeSession(session) {
  if (!session) {
    return session;
  }

  // Backend может вернуть просто GUID:
  // "01a0c9eb-000d-74f4-83c9-ae3c8302edb9"
  if (typeof session === "string") {
    return {
      id: session,
      status: "",
    };
  }

  const status = session.status
    ? String(session.status)
    : "";

  return {
    ...session,

    // History:
    // id
    //
    // Details:
    // sessionId
    id:
      session.id ??
      session.sessionId,

    status,
  };
}

export async function startBenchmark(request) {
  const response = await api.post(
    "/benchmarks",
    request
  );

  return normalizeSession(response.data);
}

export async function getHistory() {
  const response = await api.get(
    "/benchmarks"
  );

  const data = response.data;

  if (Array.isArray(data)) {
    return data.map(normalizeSession);
  }

  if (Array.isArray(data.items)) {
    return data.items.map(normalizeSession);
  }

  return [];
}

export async function getDetails(id) {
  const response = await api.get(
    `/benchmarks/${id}`
  );

  return normalizeSession(response.data);
}

export async function getComparison(sessionIds) {
  if (!sessionIds?.length) {
    return [];
  }

  const params = new URLSearchParams();

  sessionIds.forEach((id) => {
    params.append("sessionIds", id);
  });

  const response = await api.get(
    `/benchmarks/comparison?${params.toString()}`
  );

  return Array.isArray(response.data)
    ? response.data
    : [];
}

export async function cancelBenchmark(id) {
  await api.post(
    `/benchmarks/${id}/cancel`
  );
}