import { ContainerData } from "../types";
import { useNavigate } from "react-router-dom";

type Props = {
  containers: ContainerData[];
};

export default function ContainerList({ containers }: Props) {
  const navigate = useNavigate();
  return (
    <div className="overflow-x-auto">
      <table className="table w-full">
        <thead>
          <tr>
            <th>Name</th>
            <th>Action</th>
          </tr>
        </thead>
        <tbody>
          {containers.map((container, i) => (
            <tr key={i}>
              <td>{container.name}</td>
              <td>
                <button
                  className="btn btn-primary"
                  onClick={() => navigate(`/container/${container.name}`)}
                >
                  More Info
                </button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
