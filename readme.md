# kasthack.bitcoinfees

[![Nuget](https://img.shields.io/nuget/v/kasthack.bitcoinfees.svg)](https://www.nuget.org/packages/kasthack.bitcoinfees/)
[![NuGet](https://img.shields.io/nuget/dt/kasthack.bitcoinfees.svg)](https://www.nuget.org/packages/kasthack.bitcoinfees/)
[![Build status](https://img.shields.io/appveyor/ci/kasthack/kasthack-bitcoinfees/master.svg)](https://ci.appveyor.com/project/kasthack/kasthack-bitcoinfees)
[![license](https://img.shields.io/github/license/kasthack/kasthack.bitcoinfees.svg)](LICENSE)
[![Gitter](https://img.shields.io/gitter/room/nwjs/nw.js.svg)](https://gitter.im/kasthack_bitcoinfees)

## What

bitcoinfees.earn.com API client

## Installation

`Install-Package kasthack.bitcoinfees`

## Usage

Recommended fees:

`await client.Default.GetRecommendedFees().ConfigureAwait(false)`

Current fee data:

`await client.Default.GetList().ConfigureAwait(false)`

## Bugs / issues

Fork off / pull
