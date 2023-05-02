export interface StorageAccountData {
  accountName: string;
  uri: string;
  sku: string;
  accountKind: string;
}

export interface ContainerData {
  name: string;
  uri: string;
  metadata: { [key: string]: string };
}
