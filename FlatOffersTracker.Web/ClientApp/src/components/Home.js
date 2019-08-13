import React, { Component } from 'react';
import { FlatOffersOverview } from './FlatOffersOverview';

export class Home extends Component {
  static displayName = Home.name;

  render () {
    return (
      <div>
        <FlatOffersOverview/>
      </div>
    );
  }
}
