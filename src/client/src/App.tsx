import "./App.css";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { client } from "./api";
import { ContainerData, StorageAccountData } from "./types";
import AccountDataCard from "./components/AccountDataCard";
import { useState } from "react";
import ContainerList from "./components/ContainerList";

function App() {
  const [containerNameInput, setContainerNameInput] = useState<string>("");

  const queryClient = useQueryClient();

  const {
    isLoading: isFetchingAccountData,
    isError: isFetchingAccountDataError,
    data: accountData,
    error: fetchAccountDataError
  } = useQuery<StorageAccountData, Error>({
    queryKey: ["account-data"],
    queryFn: () =>
      client.get<StorageAccountData>("/account").then((res) => res.data)
  });

  const {
    isLoading: isFetchingContainers,
    isError: isFetchingContainersError,
    data: containers,
    error: fetchContainersError
  } = useQuery<ContainerData[], Error>({
    queryKey: ["container-data"],
    queryFn: () =>
      client.get<ContainerData[]>("/account/containers").then((res) => res.data)
  });

  const createContainerMutation = useMutation({
    mutationFn: (containerName: string) =>
      client.post(`/account/container/${containerName}`),
    onSuccess: () => {
      queryClient.invalidateQueries(["container-data"]);
    }
  });

  return (
    <div className="mx-auto">
      <div className="flex justify-center">
        {isFetchingAccountData ? (
          <h1 className="text-2xl font-bold mt-2">Loading...</h1>
        ) : isFetchingAccountDataError ? (
          <h1 className="text-2xl font-bold mt-2">
            Error occured :{fetchAccountDataError.message}
          </h1>
        ) : (
          <AccountDataCard accountData={accountData} />
        )}
      </div>
      <div className="mt-6 flex-col">
        <div className="space-x-4 mt-2 mb-4 max-sm:space-y-2 ">
          <input
            type="text"
            placeholder="Enter container name"
            maxLength={30}
            value={containerNameInput || ''}
            className="input input-bordered input-accent w-full max-w-xs"
            onChange={(e) => setContainerNameInput(e.target.value)}
          />
          <button
            disabled={containerNameInput.length === 0}
            className={`btn btn-accent ${
              createContainerMutation.isLoading ? "loading" : ""
            }`}
            onClick={() => {
              createContainerMutation.mutate(
                containerNameInput.replace(" ", "")
              );

              setContainerNameInput("");
            }}
          >
            Create Container
          </button>
        </div>

        <h1 className="text-2xl mt-2 font-bold place-self-">
          Account Containers:
        </h1>

        {isFetchingContainers ? (
          <h1 className="text-2xl font-bold mt-2">Loading...</h1>
        ) : isFetchingContainersError ? (
          <h1 className="text-2xl font-bold mt-2">
            Error occured :{fetchContainersError.message}
          </h1>
        ) : (
          <ContainerList containers={containers}/>
        )}
      </div>
    </div>
  );
}

export default App;
