// src/types/types.ts
export interface Auction {
  id: number;
  img: string;
  name: string;
  priceStart: string;
  startDay: string;
  startTime: string;
  timePerLap: string;
  endTime: string;
  endDay: string;
}

export interface AuctionUser {
  listAuctionID: number;
  image: string;
  nameAuction: string;
  startingPrice: string;
  startDay: string;
  startTime: string;
  endTime: string;
  endDay: string;
  statusAuction: string;
  timePerLap:string;
}

export interface AuctionDetails {
  listAuctionID: number;
  image: string;
  owner: string;
  nameAuction: string;
  moneyDeposit: number;
  description: string;
  startingPrice: number; // Use `number` if it's a numeric value
  statusAuction: string;
  startDay: string;
  startTime: string;
  endDay: string;
  endTime: string;
  numberofAuctionRounds: number;
  timePerLap: string;
  priceStep: number;
  paymentMethod: string;
  fileAuctioneer: string | null;
  signatureImg: string;
  tImage: {
    tImageId: number;
    fileAID: number;
    imange: string;
    fileAttachments: string | null;
  };
  countdowntime: string;
}

export interface AuctionDetailRegister {
  listAuctionID: number;
  category: string;
  name: string;
  image: string;
  imageEvidence: string;
  nameAuction: string;
  description: string;
  startingPrice: number;
  stepPrice : number;
  priceDeposit: number;
  startDay: string;
  startTime: string;
  timePerLap: string;
  endDay: string;
  endTime: string;
  statusAuction: string;
  countBidder: number;
}