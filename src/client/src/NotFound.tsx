import { useNavigate } from "react-router-dom";

export default function NotFound() {
  const navigate = useNavigate();
  return (
    <div className="mx-auto my-auto">
      <h1 className="text-2xl font-bold">
        The page you're looking doesn't exist
      </h1>
      <button className="btn btn-accent" onClick={() => navigate("/")}>
        Home
      </button>
    </div>
  );
}
