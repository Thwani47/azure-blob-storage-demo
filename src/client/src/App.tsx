import "./App.css";
import { useQuery } from "@tanstack/react-query";
import { client } from "./api";
import { StorageAccountData } from "./types";
import AccountDataCard from "./components/AccountDataCard";

function App() {
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

  return (
    <div className="mx-auto">
      <div className="flex justify-center">
        {isFetchingAccountData ? (
          <h1>Loading...</h1>
        ) : isFetchingAccountDataError ? (
          <h1>Error occured :{fetchAccountDataError.message}</h1>
        ) : (
          <AccountDataCard accountData={accountData}/>
        )}
      </div>
    </div>
  );
}

export default App;
