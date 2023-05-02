import axios from "axios";

export const client = axios.create({
  baseURL: import.meta.env.DEV ? 'https://localhost:7138' : 'https://azure-blob-storage-demo-api.azurewebsites.net',
  headers: {
    "Content-Type": "application/json"
  }
});
