import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { useParams, useNavigate } from "react-router-dom";
import { client } from "./api";
import { ContainerData } from "./types";
import { Fragment, useState } from "react";

type AddMetadataInput = {
  metadataKey: string;
  metadataValue: string;
};

export default function ViewContainer() {
  const { containerName } = useParams();
  const [metadataInput, setMetadataInput] = useState({ key: "", value: "" });
  const queryClient = useQueryClient();

  const navigate = useNavigate();

  const { isLoading, isError, data, error } = useQuery<ContainerData, Error>({
    queryKey: [`account-data-${containerName}`],
    queryFn: () =>
      client
        .get<ContainerData>(`/account/container/${containerName}`)
        .then((res) => res.data)
  });

  const deleteContainerMutation = useMutation({
    mutationFn: (containerName: string) =>
      client.delete(`/account/container/${containerName}`),
    onSuccess: () => {
      navigate("/");
    }
  });

  const addContainerMetadaMutation = useMutation({
    mutationFn: (input: AddMetadataInput) =>
      client.post(`/account/container/${containerName}/metadata`, {
        key: input.metadataKey,
        value: input.metadataValue
      }),
    onSuccess: () => {
      setMetadataInput({ key: "", value: "" });
      queryClient.invalidateQueries([`account-data-${containerName}`]);
    }
  });

  return (
    <div className="mx-auto">
      {isLoading ? (
        <h1 className="mt-2 text-2xl font-bold">Loading...</h1>
      ) : isError ? (
        <h1 className="mt-2 text-2xl font-bold">
          Error occured: {error.message}
        </h1>
      ) : (
        <Fragment>
          <div className="card bg-base-100 shadow-xl mx-auto">
            <div className="card-body">
              <div className="flex flex-col items-start justify-start">
                <h1 className="card-title">Container Name</h1>
                <h1> {data.name}</h1>
              </div>
              <div className="flex flex-col items-start justify-start">
                <p className="mt-2 card-title">URI: </p>
                <p>{data.uri}</p>
              </div>
              <div className="flex flex-col items-start justify-start">
                <p className="mt-2 card-title">Metadata: </p>
                <ul>
                  {Object.keys(data.metadata).map((key, index) => (
                    <li key={index}>{key}:{data.metadata[key]}</li>
                  ))}
                </ul>
              </div>
              <div className="card-actions justify-end">
                <button
                  className={`btn btn-error ${
                    deleteContainerMutation.isLoading ? "loading" : ""
                  }`}
                  onClick={() => deleteContainerMutation.mutate(data.name)}
                >
                  Delete Container
                </button>
              </div>
            </div>
          </div>
          <div className="mt-2 flex flex-col ">
            <div className="mx-auto flex space-x-2 max-sm:flex-col max-sm:space-y-2">
              <input
                type="text"
                placeholder="Key"
                value={metadataInput.key || ""}
                onChange={(e) =>
                  setMetadataInput({ ...metadataInput, key: e.target.value })
                }
                className="input w-40 max-w-xs"
              />
              <input
                type="text"
                placeholder="Value"
                value={metadataInput.value || ""}
                onChange={(e) =>
                  setMetadataInput({ ...metadataInput, value: e.target.value })
                }
                className="input w-40 max-w-xs"
              />
              <button
                className={`btn btn-accent ${
                  addContainerMetadaMutation.isLoading ? "loading" : ""
                }}`}
                disabled={
                  metadataInput.key.length === 0 ||
                  metadataInput.key.length === 0
                }
                onClick={() =>
                  addContainerMetadaMutation.mutate({
                    metadataKey: metadataInput.key.replace(" ", ""),
                    metadataValue: metadataInput.value.replace(" ", "")
                  })
                }
              >
                Add Metadata
              </button>
            </div>
          </div>
        </Fragment>
      )}
    </div>
  );
}
