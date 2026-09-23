import { api } from "./client";

export async function getAlgorithms() {
  const response = await api.get("/algorithms");

  return response.data;
}