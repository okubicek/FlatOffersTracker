import React, { Component } from 'react';
import { FlatOfferCard } from './FlatOfferCard';

export class FlatOffersOverview extends Component {
    renderFlatOffers(flatOffers) {
        return (
            <div className="row">
                {flatOffers.map(offer =>
                    <div className="col col-sm-4">
                        <FlatOfferCard flatOffer={offer} />
                    </div>
                )}
            </div>
        );
    }

    render() {
        return this.props.loading
            ? <p><em>Loading...</em></p>
            : this.renderFlatOffers(this.props.flatOffers);
    }
}