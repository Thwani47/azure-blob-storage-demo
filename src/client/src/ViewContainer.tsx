import { useQuery, useMutation } from "@tanstack/react-query";
import { useParams, useNavigate } from "react-router-dom";
import { client } from "./api";
import { ContainerData } from "./types";

export default function ViewContainer() {
  const { containerName } = useParams();

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

  return (
    <div className="mx-auto">
      {isLoading ? (
        <h1 className="mt-2 text-2xl font-bold">Loading...</h1>
      ) : isError ? (
        <h1 className="mt-2 text-2xl font-bold">
          Error occured: {error.message}
        </h1>
      ) : (
        <div className="card w-auto bg-base-100 shadow-xl mx-auto">
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
                  <li key={index}>{data.metadata[key]}</li>
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
      )}
    </div>
  );
}
