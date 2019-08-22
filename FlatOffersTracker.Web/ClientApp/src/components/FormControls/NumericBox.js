import React, { Component } from 'react';

export class NumericBox extends Component {
    constructor(props) {
        super(props);
        this.handleChange = this.handleChange.bind(this);
    }

    handleChange(e) {
        this.props.OnChange(e.target);
    }

    render() {
        return (
            <div className="form-group">
                <label>{this.props.label}</label>
                <input type="numeric"
                    value={this.props.value}
                    onChange={this.handleChange}
                    className="form-control"
                />
            </div>
        );
    }
}