import React, { Component } from 'react';
import { FlatOfferCard } from './FlatOfferCard';

export class FlatOffersOverview extends Component {
    renderFlatOffers(flatOffers) {
        return (
            <div className="row">
                {flatOffers.map(offer =>
                    <div className="col-12 col-sm-6 col-md-4">
                        <FlatOfferCard flatOffer={offer} />
                    </div>
                )}
            </div>
        );
    }

    render() {
        return this.props.loading && this.props.flatOffers.length == 0
            ? <p><em>Loading...</em></p>
            : this.renderFlatOffers(this.props.flatOffers);
    }
}