import { StorageAccountData } from "../types";

type Props = {
  accountData: StorageAccountData;
};

export default function AccountDataCard({ accountData }: Props) {
  return (
    <div className="card bg-base-100 shadow-xl">
      <div className="card-body">
        <div className="flex justify-between space-x-40">
          <h2 className="font-bold">Account Name: </h2>
          <h2 className="">{accountData.accountName}</h2>
        </div>
        <div className="flex justify-between space-x-40">
          <h1 className="font-bold">Uri:</h1>
          <h1>{accountData.uri}</h1>
        </div>
        <div className="flex justify-between space-x-40">
          <h1 className="font-bold">Sku: </h1>
          <div>{accountData.sku}</div>
        </div>
        <div className="flex justify-between space-x-40">
          <h1 className="font-bold">Storage Account Kind: </h1>
          <h1>{accountData.accountKind}</h1>
        </div>
      </div>
    </div>
  );
}
